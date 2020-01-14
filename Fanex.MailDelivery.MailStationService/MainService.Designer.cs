using System.Diagnostics.CodeAnalysis;

namespace Fanex.MailDelivery.MailStationService
{   
    partial class MainService
    {       
        private System.ComponentModel.IContainer _components = null;
      
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _components = new System.ComponentModel.Container();
            this.ServiceName = "Fanex.MailStationService";
        }

        #endregion
    }
}
