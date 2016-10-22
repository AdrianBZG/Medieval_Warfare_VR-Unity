
//
// This file is auto-generated. Please don't modify it!
//
using System;
using System.Runtime.InteropServices;

namespace OpenCVForUnity
{


// C++: class CLAHE
		public class CLAHE : Algorithm
		{


				protected override void Dispose (bool disposing)
				{
						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
						try {

								if (disposing) {
								}

								if (IsEnabledDispose) {
										if (nativeObj != IntPtr.Zero)
												imgproc_CLAHE_delete (nativeObj);
										nativeObj = IntPtr.Zero;
								}
				
						} finally {
								base.Dispose (disposing);
						}
			
						#else
						return;
						#endif
				}
	
				protected CLAHE (IntPtr addr) : base(addr)
				{
				}
	
	
				//
				// C++:  void CLAHE::apply(Mat src, Mat& dst)
				//
	
				public  void apply (Mat src, Mat dst)
				{
						if (src != null)
								src.ThrowIfDisposed ();
						if (dst != null)
								dst.ThrowIfDisposed ();
						ThrowIfDisposed ();


						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						imgproc_CLAHE_apply_10 (nativeObj, src.nativeObj, dst.nativeObj);
		
						return;
						#else
						return;
						#endif
				}
	
	
				//
				// C++:  void CLAHE::setClipLimit(double clipLimit)
				//
	
				public  void setClipLimit (double clipLimit)
				{
						ThrowIfDisposed ();

						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						imgproc_CLAHE_setClipLimit_10 (nativeObj, clipLimit);
		
						return;
						#else
						return;
						#endif
				}
	
	
				//
				// C++:  void CLAHE::setTilesGridSize(Size tileGridSize)
				//
	
				public  void setTilesGridSize (Size tileGridSize)
				{
						ThrowIfDisposed ();
					
						#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
						imgproc_CLAHE_setTilesGridSize_10 (nativeObj, tileGridSize.width, tileGridSize.height);
		
						return;
						#else
						return;
						#endif
				}
	
	
	

		#if UNITY_IOS && !UNITY_EDITOR
		// C++:  void CLAHE::apply(Mat src, Mat& dst)
		[DllImport("__Internal")]
		private static extern void imgproc_CLAHE_apply_10(IntPtr nativeObj, IntPtr src_nativeObj, IntPtr dst_nativeObj);
		
		// C++:  void CLAHE::setClipLimit(double clipLimit)
		[DllImport("__Internal")]
		private static extern void imgproc_CLAHE_setClipLimit_10(IntPtr nativeObj, double clipLimit);
		
		// C++:  void CLAHE::setTilesGridSize(Size tileGridSize)
		[DllImport("__Internal")]
		private static extern void imgproc_CLAHE_setTilesGridSize_10(IntPtr nativeObj, double tileGridSize_width, double tileGridSize_height);
		
		// native support for java finalize()
		[DllImport("__Internal")]
		private static extern void imgproc_CLAHE_delete(IntPtr nativeObj);

#else
		
				// C++:  void CLAHE::apply(Mat src, Mat& dst)
				[DllImport("opencvforunity")]
				private static extern void imgproc_CLAHE_apply_10 (IntPtr nativeObj, IntPtr src_nativeObj, IntPtr dst_nativeObj);
		
				// C++:  void CLAHE::setClipLimit(double clipLimit)
				[DllImport("opencvforunity")]
				private static extern void imgproc_CLAHE_setClipLimit_10 (IntPtr nativeObj, double clipLimit);
		
				// C++:  void CLAHE::setTilesGridSize(Size tileGridSize)
				[DllImport("opencvforunity")]
				private static extern void imgproc_CLAHE_setTilesGridSize_10 (IntPtr nativeObj, double tileGridSize_width, double tileGridSize_height);
		
				// native support for java finalize()
				[DllImport("opencvforunity")]
				private static extern void imgproc_CLAHE_delete (IntPtr nativeObj);
		#endif
		}
}
