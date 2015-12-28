using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using Acr.IO;
using System.Reflection;
using System.Text;
using System.IO;

namespace librarian
{
	public partial class Main : ContentPage
	{
		ZXingScannerPage scanPage;
		IFile file;

		public Main ()
		{
			InitializeComponent ();
			FileSetup ();
		}

		async void FileSetup() {
			file = FileSystem.Instance.Public.GetFile("barcodes.txt");
			if (!file.Exists) {
				file.Create ();
			}

//			var assembly = Assembly.Load(new AssemblyName("librarian"));
//			using (var stream = assembly.GetManifestResourceStream("test.pdf"))
//			using (var fs = file.OpenWrite())
//				stream.CopyTo(fs);
//
//			if (!FileViewer.Instance.Open(file))
//				await DisplayAlert("ERROR", "Could not open file " + file.FullName, "OK");
		}
//
//		public static bool SaveImage(string filename, byte[] image) {
//			try {
//				CreatePicturesFolder();
//				var folder = FileSystem.Instance.GetDirectory(System.IO.Path.Combine(FileSystem.Instance.Cache.FullName, "Pictures"));
//				var photo = folder.CreateFile(filename);
//				using(var p = photo.OpenWrite()) {
//					using(var m = new MemoryStream(image)) {
//						//new MemoryStream(image).CopyTo(p);
//						m.CopyTo(p);
//					}
//				}
//				return true;
//			}
//			catch {
//				return false;
//			}
//		}
//
		async void OnClicked_ScanOnce (object sender, EventArgs e) {
			scanPage = new ZXingScannerPage ();
			scanPage.OnScanResult += (result) => {
				scanPage.IsScanning = false;

				Device.BeginInvokeOnMainThread (() => {
					Navigation.PopAsync ();
					DisplayAlert("Scanned Barcode", result.Text, "OK");
					UpdateFile(result.Text);
				});
			};

			await Navigation.PushAsync (scanPage);
		}

		async void OnClicked_ScanContinuously (object sender, EventArgs e) {
			scanPage = new ZXingScannerPage ();
			scanPage.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread (() => 
					DisplayAlert ("Scanned Barcode", result.Text, "OK"));

			await Navigation.PushAsync (scanPage);
		}

		void UpdateFile(string barcode) {
			if (!String.IsNullOrEmpty(barcode)) {
				byte[] barcodeAsBytes = Encoding.GetEncoding ("UTF8").GetBytes (barcode.ToCharArray ());
				using (var f = file.OpenWrite ()) {
					using (var m = new MemoryStream (barcodeAsBytes)) {
						m.CopyTo (f);
					}
				}
			}
		}

		void OnClicked_ViewHistory (object sender, EventArgs e) {
			Navigation.PushAsync (new History ());
		}

		void OnClicked_ShareHistory (object sender, EventArgs e) {
//			var activityController = new UIActivityViewController(new NSObject[] { UIActivity.FromObject(textToShare) }, null);
//			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(activityController, true, null);

		}
	}
}

