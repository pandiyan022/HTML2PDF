using System;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Java.IO;
using Java.Lang;
using static Android.Print.PrintDocumentAdapter;

namespace android.print
{
    public interface PdfPrintListener
    {
        void onWriteFinished(string output);
        void onError();
    }
    public class MyLayoutResultCallback : LayoutResultCallback
    {
        PrintDocumentAdapter printDocumentAdapter;
        public MyLayoutResultCallback(PrintDocumentAdapter printDocumentAdapter ,IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            this.printDocumentAdapter = printDocumentAdapter;
        }

        public override void OnLayoutCancelled()
        {
            base.OnLayoutCancelled();
        }

        public override void OnLayoutFailed(ICharSequence error)
        {
            base.OnLayoutFailed(error);
        }

        public override void OnLayoutFinished(PrintDocumentInfo info, bool changed)
        {

            File sdf = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            ParcelFileDescriptor fileDescriptor = GetOutputFile(sdf, "SMV_Report");
            printDocumentAdapter.OnWrite(new PageRange[] { PageRange.AllPages }, fileDescriptor, new CancellationSignal(),
             new MyWriteResultCallbackt(JNIEnv.Handle, JniHandleOwnership.DoNotRegister));

            base.OnLayoutFinished(info, changed);
        }



        public ParcelFileDescriptor GetOutputFile(File path, string fileName)
        {
            bool success = true;
            if (!path.Exists())
            {
                success = path.Mkdir();
            }
            if (success)
            {
                File file = new File(path, fileName);
                try
                {
                    success = file.CreateNewFile();
                    if (success)
                    {
                        return ParcelFileDescriptor.Open(file, ParcelFileMode.ReadWrite);
                    }

                }
                catch (System.Exception e)
                {
                    //AnalyticsHandlerAdapter.getInstance().sendException(e);
                }
            }
            return null;
        }
    }





    public class MyWriteResultCallbackt : WriteResultCallback
    {
        public MyWriteResultCallbackt(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override void OnWriteFinished(PageRange[] pages)
        {

            base.OnWriteFinished(pages);
        }
    }
    public partial class PdfPrint
    {
        private static PrintAttributes printAttributes;
        private static PdfPrintListener mListener;
        private static PrintDocumentAdapter printDocumentAdapter;

        public void setPdfPrintListener(PdfPrintListener listener)
        {
            mListener = listener;
        }


        public PdfPrint(PrintAttributes pAttributes)
        {
            printAttributes = pAttributes;
        }

        public PdfPrint(PrintAttributes printAttributes, PdfPrintListener Listener) : this(printAttributes)
        {
            mListener = Listener;
        }

        public void Print(PrintDocumentAdapter printAdapter)
        {
            printDocumentAdapter = printAdapter;


         

            printDocumentAdapter.OnLayout(null, printAttributes, null,    new MyLayoutResultCallback(printAdapter,JNIEnv.Handle, JniHandleOwnership.DoNotRegister), null);

        }

        //PrintDocumentAdapter.LayoutResultCallback callback =  delegate (()=>{

       

    }
}
