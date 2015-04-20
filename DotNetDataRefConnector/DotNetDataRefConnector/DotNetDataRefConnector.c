#define IBM 1
#define VERSION "0.2"

#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <Windows.h>
#include <memory.h>
#include "XPLMDataAccess.h"
#include "XPLMProcessing.h"


// *****************************************************************************************************
// *** this programm ist used for interface X-Plane DataRef SDK commands to .Net dll over shared memory
// *** created by Alexander Eckers - 2013
// *****************************************************************************************************


// command shared memory
static LPVOID lpCommandObjectMem = NULL;
static HANDLE hdCommandObjectMap = NULL;
static HANDLE hdCommandObjectEvent = NULL;

// data shared memory
static LPVOID lpDataObjectMem = NULL;
static HANDLE hdDataObjectMap = NULL;
static HANDLE hdDataObjectEvent = NULL;

// main loop
static HANDLE hdMainLoopThread = NULL;
static DWORD  dwMainLoopThreadId = NULL;

// flag for exit loop
static BOOL ExitLoop = FALSE;

// Commandlist
#define CMD_XPLMCanWriteDataRef	0x00
#define CMD_XPLMGetDataRefTypes	0x01
#define CMD_XPLMGetDatai 0x02
#define CMD_XPLMSetDatai 0x03
#define CMD_XPLMGetDataf 0x04
#define CMD_XPLMSetDataf 0x05
#define CMD_XPLMGetDatad 0x06
#define CMD_XPLMSetDatad 0x07
#define CMD_XPLMGetDatavi 0x08
#define CMD_XPLMSetDatavi 0x09
#define CMD_XPLMGetDatavf 0x0a
#define CMD_XPLMSetDatavf 0x0b
#define CMD_XPLMGetDatab 0x0c
#define CMD_XPLMSetDatab 0x0d
#define CMD_XPLMFindDataRef 0x0e

// datatype list
#define dtype_int 0x00		// 2 bytes
#define dtype_float 0x01	// 4 bytes
#define dtype_double 0x02	// 8 bytes
#define dtype_handle 0x03	// 4 bytes
#define dtype_float_array 0x04	// x*4 bytes
#define dtype_int_array 0x05	// x*2 bytes
#define dtype_byte_array 0x06	// x bytes

// command exchange struct (only for read)
typedef struct CommandExchange
{
	// used to identify the command (only for read)
	char Command;
	// used for DataRef Name (only for read)
	char XPLMDataRefName[128];
	// used for DataRef Handle ( only for read) 
	long int *XPLMDataRefHandle;
} smCommandExchange;

// data exchange struct (used for read/write data)
typedef struct DataExchange
{
	// length(in bytes) of data in buffer (read/write)
	long int DataLength; 
	// indentify the datatype (read/write)
	char DataType; 
	// buffer for data (read/write)
	char Data[65536];
} smDataExchange;

// converters
//#define GETBYTE0(v)   (*((unsigned char *) (&v)))

// methods
BOOL SetupSharedMemory(void);
DWORD WINAPI MainLoop(LPVOID lpParam);

// this method initialize the shared memory for command
// and data exchange through .net dll
BOOL SetupSharedMemory()
{
	// get size of needed memory
	UINT smCommandExchangeSize = sizeof(smCommandExchange);
	UINT smDataExchangeSize = sizeof(smDataExchange);

	// open memory file mapping
	hdCommandObjectMap = CreateFileMapping(INVALID_HANDLE_VALUE,NULL,PAGE_READWRITE,0,smCommandExchangeSize,TEXT("DNDRC_COMMAND")); 
	if (hdCommandObjectMap == NULL) 
                return FALSE; 

	hdDataObjectMap = CreateFileMapping(INVALID_HANDLE_VALUE,NULL,PAGE_READWRITE,0,smDataExchangeSize,TEXT("DNDRC_DATA")); 
	if (hdDataObjectMap == NULL) 
                return FALSE; 

	// create events for data exchange
	hdCommandObjectEvent = CreateEvent(NULL, TRUE, FALSE, TEXT("DNDRC_COMMAND_EVENT"));
	hdDataObjectEvent = CreateEvent(NULL, TRUE, FALSE, TEXT("DNDRC_DATA_EVENT"));

	// create a view to file mapping
	lpCommandObjectMem = MapViewOfFile(hdCommandObjectMap, FILE_MAP_WRITE,0,0,0);	
	if ( lpCommandObjectMem == NULL) 
            return FALSE; 

	lpDataObjectMem = MapViewOfFile(hdDataObjectMap,FILE_MAP_WRITE,0,0,0);					
	if ( lpDataObjectMem == NULL) 
                return FALSE; 

	// set memory to 0
	memset(lpCommandObjectMem, '\0', smCommandExchangeSize);
	memset(lpDataObjectMem, '\0', smDataExchangeSize);

	// everything ok
	return TRUE;
}


DWORD WINAPI MainLoop(LPVOID lpParam)
{
	DWORD Timer;
	XPLMDataRef dataRef;
	smCommandExchange cmd;
	smDataExchange cmdData;
	smDataExchange resultData;

	while(ExitLoop!=TRUE)
	{
		Timer = WaitForSingleObject(hdCommandObjectEvent, 1000);
		if(Timer != WAIT_TIMEOUT)
		{

			resultData.DataLength=0;

			ResetEvent(hdCommandObjectEvent);
			memcpy(&cmd, lpCommandObjectMem, sizeof(smCommandExchange));
			memcpy(&cmdData, lpDataObjectMem, sizeof(smDataExchange));
			
			if(cmd.XPLMDataRefHandle>0)
			{
				dataRef=cmd.XPLMDataRefHandle;
			}
			else{
				dataRef=XPLMFindDataRef(cmd.XPLMDataRefName);
			}
			
			if(dataRef!=NULL)
			{
				switch(cmd.Command)
				{
					case CMD_XPLMFindDataRef:
						{
							memcpy(resultData.Data,&dataRef,4);
							resultData.DataLength=4;
							resultData.DataType=dtype_handle;
							break;
						}
					case CMD_XPLMCanWriteDataRef:
						{
							int ret=XPLMCanWriteDataRef(dataRef);
							memcpy(resultData.Data,&ret,2);
							resultData.DataLength=2;
							resultData.DataType=dtype_int;
							break;
						}
					case CMD_XPLMGetDataRefTypes:
						{
							XPLMDataTypeID ret=XPLMGetDataRefTypes(dataRef);
							memcpy(resultData.Data,&ret,2);
							resultData.DataLength=2;
							resultData.DataType=dtype_int;
							break;
						}
					case CMD_XPLMGetDatai:
						{
							int ret=XPLMGetDatai(dataRef);
							memcpy(resultData.Data,&ret,2);
							resultData.DataLength=2;  
							resultData.DataType=dtype_int;
							break;
						}
					case CMD_XPLMSetDatai:
						{
							if((cmdData.DataType==dtype_int) && (cmdData.DataLength==2))
							{
								int *value=(int*)cmdData.Data;
								XPLMSetDatai(dataRef,*value);
							}
							break;
						}
					case CMD_XPLMGetDataf:
						{
							float ret=XPLMGetDataf(dataRef);
							memcpy(resultData.Data,&ret,4);
							resultData.DataLength=4;
							resultData.DataType=dtype_float;
							break;
						}
					case CMD_XPLMSetDataf:
							if((cmdData.DataType==dtype_float) && (cmdData.DataLength==4))
							{
								float *value=(float*)cmdData.Data;
								XPLMSetDataf(dataRef,*value);
							}
							break;
					case CMD_XPLMGetDatad:
						{
							double ret=XPLMGetDatad(dataRef);
							memcpy(resultData.Data,&ret,8);
							resultData.DataLength=8;
							resultData.DataType=dtype_double;
							break;
						}
					case CMD_XPLMSetDatad:
						{
							if((cmdData.DataType==dtype_double) && (cmdData.DataLength==8))
							{
								double *value=(double*)cmdData.Data;
								XPLMSetDatad(dataRef,*value);
							}
							break;
						}
					case CMD_XPLMGetDatavi:
						{
							int size = XPLMGetDatavi(dataRef,NULL,0,0);
							if(size<=32768)
							{
								int *intVals = (int*)malloc(size * sizeof *intVals);
								int ret=0;
								ret=XPLMGetDatavi(dataRef,intVals,0,size);
								if(ret>0)
								{
									memcpy(resultData.Data,intVals,2*ret);
									resultData.DataLength=2*ret;
									resultData.DataType=dtype_int_array;
								}
							}
							break;
						}
					case CMD_XPLMSetDatavi:
						{
							if((cmdData.DataType==dtype_int_array) && (cmdData.DataLength>0))
							{
								int size = cmdData.DataLength /2;
								int *intVals = (int*)malloc(size * sizeof *intVals);
								memcpy(intVals, cmdData.Data,size * 2);
								XPLMSetDatavi(dataRef,intVals,0,size);
							}
							break;
						}
					case CMD_XPLMGetDatavf:
						{
							int size = XPLMGetDatavf(dataRef,NULL,0,0);
							if(size<=16384)
							{
								float *floatVals = (float*)malloc(size * sizeof *floatVals);
								int ret=0;
								ret=XPLMGetDatavf(dataRef,floatVals,0,size);
								if(ret>0)
								{
									memcpy(resultData.Data,floatVals,4*ret);
									resultData.DataLength=4*ret;
									resultData.DataType=dtype_float_array;
								}
							}
							break;
						}
					case CMD_XPLMSetDatavf:
						{
							if((cmdData.DataType==dtype_float_array) && (cmdData.DataLength>0))
							{
								int size = cmdData.DataLength /4;
								float *floatVals = (float*)malloc(size * sizeof *floatVals);
								memcpy(floatVals, cmdData.Data,size * 4);
								XPLMSetDatavf(dataRef,floatVals,0,size);
							
							}
							break;
						}
					case CMD_XPLMGetDatab:
						{
							int size = XPLMGetDatab(dataRef,NULL,0,0);
							if(size<=65536)
							{
								char *byteVals = (char*)malloc(size * sizeof *byteVals);
								int ret=0;
								ret=XPLMGetDatab(dataRef,byteVals,0,size);
								if(ret>0)
								{
									memcpy(resultData.Data,byteVals,ret);
									resultData.DataLength=ret;
									resultData.DataType=dtype_byte_array;
								}
							}
							break;
						}
					case CMD_XPLMSetDatab:
						{
							if((cmdData.DataType==dtype_byte_array) && (cmdData.DataLength>0))
							{
								XPLMSetDatab(dataRef,cmdData.Data,0,cmdData.DataLength);
							}
							break;
						}
					default:
						resultData.DataLength=0;
						break;
				} //switch
			}
			memcpy(lpDataObjectMem, &resultData, sizeof(smDataExchange));
			SetEvent(hdDataObjectEvent);
		
		} // if Timeout
	} // while
	return 1;
}


PLUGIN_API int XPluginStart(char * outName,char * outSig,char * outDesc)
{
	strcpy(outName, "DotNet DataRef Connector");
	strcpy(outSig, "DotNetDataRefConnector");
	strcpy(outDesc, "Connects X-Plane DataRef to DotNet over shared Memory");

	SetupSharedMemory();

	hdMainLoopThread = CreateThread(NULL,0,MainLoop,NULL,0,&dwMainLoopThreadId);
}

PLUGIN_API void	XPluginStop(void)
{
	ExitLoop = TRUE;

	WaitForSingleObject(hdMainLoopThread, INFINITE);

	UnmapViewOfFile(lpCommandObjectMem);
	UnmapViewOfFile(lpDataObjectMem);

	CloseHandle(hdCommandObjectMap);
	CloseHandle(hdDataObjectMap);
}             

