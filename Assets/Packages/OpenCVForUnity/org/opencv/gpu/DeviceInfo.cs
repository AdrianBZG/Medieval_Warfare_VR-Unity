using System;
using System.Runtime.InteropServices;

namespace OpenCVForUnity
{
	

// C++: class DeviceInfo
		public class DeviceInfo : DisposableOpenCVObject
		{


				protected override void Dispose (bool disposing)
				{
						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
						try {

								if (disposing) {
								}

								if (IsEnabledDispose) {
										if (nativeObj != IntPtr.Zero)
												gpu_DeviceInfo_delete (nativeObj);
										nativeObj = IntPtr.Zero;
								}
				
						} finally {
								base.Dispose (disposing);
						}
			
						#else
						return;
						#endif
				}
	
//	protected readonly IntPtr nativeObj;
				protected DeviceInfo (IntPtr addr)
				{
						nativeObj = addr;
				}
	
	
				//
				// C++:   DeviceInfo::DeviceInfo()
				//
	
				public   DeviceInfo ()
				{
						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						nativeObj = gpu_DeviceInfo_DeviceInfo_10 ();
		
						return;
						#else
						return;
						#endif
				}
	
	
				//
				// C++:   DeviceInfo::DeviceInfo(int device_id)
				//
	
				public   DeviceInfo (int device_id)
				{
						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						nativeObj = gpu_DeviceInfo_DeviceInfo_11 (device_id);
		
						return;
						#else
						return;
						#endif
				}
	
	
				//
				// C++:  int DeviceInfo::deviceID()
				//
	
				public  int deviceID ()
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						int retVal = gpu_DeviceInfo_deviceID_10 (nativeObj);
		
						return retVal;
						#else
						return 0;
						#endif
				}
	
	
				//
				// C++:  size_t DeviceInfo::freeMemory()
				//
	
				public  long freeMemory ()
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						long retVal = gpu_DeviceInfo_freeMemory_10 (nativeObj);//TODO: @size_t long long
		
						return retVal;
						#else
						return 0;
						#endif
				}
	
	
				//
				// C++:  bool DeviceInfo::isCompatible()
				//
	
				public  bool isCompatible ()
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						bool retVal = gpu_DeviceInfo_isCompatible_10 (nativeObj);
		
						return retVal;
						#else
						return false;
						#endif
				}
	
	
				//
				// C++:  int DeviceInfo::majorVersion()
				//
	
				public  int majorVersion ()
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						int retVal = gpu_DeviceInfo_majorVersion_10 (nativeObj);
		
						return retVal;
						#else
						return 0;
						#endif
				}
	
	
				//
				// C++:  int DeviceInfo::minorVersion()
				//
	
				public  int minorVersion ()
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						int retVal = gpu_DeviceInfo_minorVersion_10 (nativeObj);
		
						return retVal;
						#else
						return 0;
						#endif
				}
	
	
				//
				// C++:  int DeviceInfo::multiProcessorCount()
				//
	
				public  int multiProcessorCount ()
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						int retVal = gpu_DeviceInfo_multiProcessorCount_10 (nativeObj);
		
						return retVal;
						#else
						return 0;
						#endif
				}
	
	
				//
				// C++:  string DeviceInfo::name()
				//
	
				public  string name ()
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						string retVal = Marshal.PtrToStringAnsi (gpu_DeviceInfo_name_10 (nativeObj));
		
						return retVal;
						#else
						return null;
						#endif
				}
	
	
				//
				// C++:  void DeviceInfo::queryMemory(size_t& totalMemory, size_t& freeMemory)
				//
	
				public  void queryMemory (long totalMemory, long freeMemory)
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						double[] totalMemory_out = new double[1];
						double[] freeMemory_out = new double[1];
						gpu_DeviceInfo_queryMemory_10 (nativeObj, totalMemory_out, freeMemory_out);
						totalMemory = (long)totalMemory_out [0];
						freeMemory = (long)freeMemory_out [0];
						#else
						return;
						#endif
				}
	
	
				//
				// C++:  size_t DeviceInfo::sharedMemPerBlock()
				//
	
				public  long sharedMemPerBlock ()
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						long retVal = gpu_DeviceInfo_sharedMemPerBlock_10 (nativeObj);//TODO: @size_t long->double 
		
						return retVal;
						#else
						return 0;
						#endif
				}
	
	
				//
				// C++:  bool DeviceInfo::supports(int feature_set)
				//
	
				public  bool supports (int feature_set)
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						bool retVal = gpu_DeviceInfo_supports_10 (nativeObj, feature_set);
		
						return retVal;
						#else
						return false;
						#endif
				}
	
	
				//
				// C++:  size_t DeviceInfo::totalMemory()
				//
	
				public  long totalMemory ()
				{
						ThrowIfDisposed ();
					
						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						long retVal = gpu_DeviceInfo_totalMemory_10 (nativeObj);//TODO: @size_t long->double
		
						return retVal;
						#else
						return 0;
						#endif
				}
	
	
	

		#if UNITY_IOS && !UNITY_EDITOR
		// C++:   DeviceInfo::DeviceInfo()
		[DllImport("__Internal")]
		private static extern IntPtr gpu_DeviceInfo_DeviceInfo_10();
		
		// C++:   DeviceInfo::DeviceInfo(int device_id)
		[DllImport("__Internal")]
		private static extern IntPtr gpu_DeviceInfo_DeviceInfo_11(int device_id);
		
		// C++:  int DeviceInfo::deviceID()
		[DllImport("__Internal")]
		private static extern int gpu_DeviceInfo_deviceID_10(IntPtr nativeObj);
		
		// C++:  size_t DeviceInfo::freeMemory()
		[DllImport("__Internal")]
		private static extern long gpu_DeviceInfo_freeMemory_10(IntPtr nativeObj);
		
		// C++:  bool DeviceInfo::isCompatible()
		[DllImport("__Internal")]
		private static extern bool gpu_DeviceInfo_isCompatible_10(IntPtr nativeObj);
		
		// C++:  int DeviceInfo::majorVersion()
		[DllImport("__Internal")]
		private static extern int gpu_DeviceInfo_majorVersion_10(IntPtr nativeObj);
		
		// C++:  int DeviceInfo::minorVersion()
		[DllImport("__Internal")]
		private static extern int gpu_DeviceInfo_minorVersion_10(IntPtr nativeObj);
		
		// C++:  int DeviceInfo::multiProcessorCount()
		[DllImport("__Internal")]
		private static extern int gpu_DeviceInfo_multiProcessorCount_10(IntPtr nativeObj);
		
		// C++:  string DeviceInfo::name()
		[DllImport("__Internal")]
		private static extern IntPtr gpu_DeviceInfo_name_10(IntPtr nativeObj);
		
		// C++:  void DeviceInfo::queryMemory(size_t& totalMemory, size_t& freeMemory)
		[DllImport("__Internal")]
		private static extern void gpu_DeviceInfo_queryMemory_10(IntPtr nativeObj,[In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] double[] totalMemory_out,[In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] double[] freeMemory_out);
		
		// C++:  size_t DeviceInfo::sharedMemPerBlock()
		[DllImport("__Internal")]
		private static extern long gpu_DeviceInfo_sharedMemPerBlock_10(IntPtr nativeObj);
		
		// C++:  bool DeviceInfo::supports(int feature_set)
		[DllImport("__Internal")]
		private static extern bool gpu_DeviceInfo_supports_10(IntPtr nativeObj, int feature_set);
		
		// C++:  size_t DeviceInfo::totalMemory()
		[DllImport("__Internal")]
		private static extern long gpu_DeviceInfo_totalMemory_10(IntPtr nativeObj);
		
		// native support for java finalize()
		[DllImport("__Internal")]
		private static extern void gpu_DeviceInfo_delete(IntPtr nativeObj);

#else
		
				// C++:   DeviceInfo::DeviceInfo()
				[DllImport("opencvforunity")]
				private static extern IntPtr gpu_DeviceInfo_DeviceInfo_10 ();
		
				// C++:   DeviceInfo::DeviceInfo(int device_id)
				[DllImport("opencvforunity")]
				private static extern IntPtr gpu_DeviceInfo_DeviceInfo_11 (int device_id);
		
				// C++:  int DeviceInfo::deviceID()
				[DllImport("opencvforunity")]
				private static extern int gpu_DeviceInfo_deviceID_10 (IntPtr nativeObj);
		
				// C++:  size_t DeviceInfo::freeMemory()
				[DllImport("opencvforunity")]
				private static extern long gpu_DeviceInfo_freeMemory_10 (IntPtr nativeObj);
		
				// C++:  bool DeviceInfo::isCompatible()
				[DllImport("opencvforunity")]
				private static extern bool gpu_DeviceInfo_isCompatible_10 (IntPtr nativeObj);
		
				// C++:  int DeviceInfo::majorVersion()
				[DllImport("opencvforunity")]
				private static extern int gpu_DeviceInfo_majorVersion_10 (IntPtr nativeObj);
		
				// C++:  int DeviceInfo::minorVersion()
				[DllImport("opencvforunity")]
				private static extern int gpu_DeviceInfo_minorVersion_10 (IntPtr nativeObj);
		
				// C++:  int DeviceInfo::multiProcessorCount()
				[DllImport("opencvforunity")]
				private static extern int gpu_DeviceInfo_multiProcessorCount_10 (IntPtr nativeObj);
		
				// C++:  string DeviceInfo::name()
				[DllImport("opencvforunity")]
				private static extern IntPtr gpu_DeviceInfo_name_10 (IntPtr nativeObj);
		
				// C++:  void DeviceInfo::queryMemory(size_t& totalMemory, size_t& freeMemory)
				[DllImport("opencvforunity")]
				private static extern void gpu_DeviceInfo_queryMemory_10 (IntPtr nativeObj, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] double[] totalMemory_out, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] double[] freeMemory_out);
		
				// C++:  size_t DeviceInfo::sharedMemPerBlock()
				[DllImport("opencvforunity")]
				private static extern long gpu_DeviceInfo_sharedMemPerBlock_10 (IntPtr nativeObj);
		
				// C++:  bool DeviceInfo::supports(int feature_set)
				[DllImport("opencvforunity")]
				private static extern bool gpu_DeviceInfo_supports_10 (IntPtr nativeObj, int feature_set);
		
				// C++:  size_t DeviceInfo::totalMemory()
				[DllImport("opencvforunity")]
				private static extern long gpu_DeviceInfo_totalMemory_10 (IntPtr nativeObj);
		
				// native support for java finalize()
				[DllImport("opencvforunity")]
				private static extern void gpu_DeviceInfo_delete (IntPtr nativeObj);
		#endif
		}
}