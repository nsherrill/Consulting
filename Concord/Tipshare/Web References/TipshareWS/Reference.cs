﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Tipshare.TipshareWS {
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="TipshareWSSoap", Namespace="http://tempuri.org/")]
    public partial class TipshareWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetTipshareVersionOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetProgramUsageOperationCompleted;
        
        private System.Threading.SendOrPostCallback RecordProgramUsageOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetUndistributedStoresOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetListOfCardsToSetAsUnallocatedOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRequestsOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateRequestOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRecentCompletedOperationCompleted;
        
        private System.Threading.SendOrPostCallback InsertResultsOperationCompleted;
        
        private System.Threading.SendOrPostCallback LastCompletedResultDateOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public TipshareWS() {
            this.Url = global::Tipshare.Properties.Settings.Default.Tipshare_TipshareWS_TipshareWS;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetTipshareVersionCompletedEventHandler GetTipshareVersionCompleted;
        
        /// <remarks/>
        public event GetProgramUsageCompletedEventHandler GetProgramUsageCompleted;
        
        /// <remarks/>
        public event RecordProgramUsageCompletedEventHandler RecordProgramUsageCompleted;
        
        /// <remarks/>
        public event GetUndistributedStoresCompletedEventHandler GetUndistributedStoresCompleted;
        
        /// <remarks/>
        public event GetListOfCardsToSetAsUnallocatedCompletedEventHandler GetListOfCardsToSetAsUnallocatedCompleted;
        
        /// <remarks/>
        public event GetRequestsCompletedEventHandler GetRequestsCompleted;
        
        /// <remarks/>
        public event UpdateRequestCompletedEventHandler UpdateRequestCompleted;
        
        /// <remarks/>
        public event GetRecentCompletedCompletedEventHandler GetRecentCompletedCompleted;
        
        /// <remarks/>
        public event InsertResultsCompletedEventHandler InsertResultsCompleted;
        
        /// <remarks/>
        public event LastCompletedResultDateCompletedEventHandler LastCompletedResultDateCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetTipshareVersion", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Version GetTipshareVersion() {
            object[] results = this.Invoke("GetTipshareVersion", new object[0]);
            return ((Version)(results[0]));
        }
        
        /// <remarks/>
        public void GetTipshareVersionAsync() {
            this.GetTipshareVersionAsync(null);
        }
        
        /// <remarks/>
        public void GetTipshareVersionAsync(object userState) {
            if ((this.GetTipshareVersionOperationCompleted == null)) {
                this.GetTipshareVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTipshareVersionOperationCompleted);
            }
            this.InvokeAsync("GetTipshareVersion", new object[0], this.GetTipshareVersionOperationCompleted, userState);
        }
        
        private void OnGetTipshareVersionOperationCompleted(object arg) {
            if ((this.GetTipshareVersionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTipshareVersionCompleted(this, new GetTipshareVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetProgramUsage", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] GetProgramUsage(System.DateTime DateStart, string UserPass) {
            object[] results = this.Invoke("GetProgramUsage", new object[] {
                        DateStart,
                        UserPass});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void GetProgramUsageAsync(System.DateTime DateStart, string UserPass) {
            this.GetProgramUsageAsync(DateStart, UserPass, null);
        }
        
        /// <remarks/>
        public void GetProgramUsageAsync(System.DateTime DateStart, string UserPass, object userState) {
            if ((this.GetProgramUsageOperationCompleted == null)) {
                this.GetProgramUsageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetProgramUsageOperationCompleted);
            }
            this.InvokeAsync("GetProgramUsage", new object[] {
                        DateStart,
                        UserPass}, this.GetProgramUsageOperationCompleted, userState);
        }
        
        private void OnGetProgramUsageOperationCompleted(object arg) {
            if ((this.GetProgramUsageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetProgramUsageCompleted(this, new GetProgramUsageCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RecordProgramUsage", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string RecordProgramUsage(string StoreName) {
            object[] results = this.Invoke("RecordProgramUsage", new object[] {
                        StoreName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void RecordProgramUsageAsync(string StoreName) {
            this.RecordProgramUsageAsync(StoreName, null);
        }
        
        /// <remarks/>
        public void RecordProgramUsageAsync(string StoreName, object userState) {
            if ((this.RecordProgramUsageOperationCompleted == null)) {
                this.RecordProgramUsageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecordProgramUsageOperationCompleted);
            }
            this.InvokeAsync("RecordProgramUsage", new object[] {
                        StoreName}, this.RecordProgramUsageOperationCompleted, userState);
        }
        
        private void OnRecordProgramUsageOperationCompleted(object arg) {
            if ((this.RecordProgramUsageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RecordProgramUsageCompleted(this, new RecordProgramUsageCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetUndistributedStores", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetUndistributedStores() {
            object[] results = this.Invoke("GetUndistributedStores", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetUndistributedStoresAsync() {
            this.GetUndistributedStoresAsync(null);
        }
        
        /// <remarks/>
        public void GetUndistributedStoresAsync(object userState) {
            if ((this.GetUndistributedStoresOperationCompleted == null)) {
                this.GetUndistributedStoresOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetUndistributedStoresOperationCompleted);
            }
            this.InvokeAsync("GetUndistributedStores", new object[0], this.GetUndistributedStoresOperationCompleted, userState);
        }
        
        private void OnGetUndistributedStoresOperationCompleted(object arg) {
            if ((this.GetUndistributedStoresCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetUndistributedStoresCompleted(this, new GetUndistributedStoresCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetListOfCardsToSetAsUnallocated", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] GetListOfCardsToSetAsUnallocated() {
            object[] results = this.Invoke("GetListOfCardsToSetAsUnallocated", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void GetListOfCardsToSetAsUnallocatedAsync() {
            this.GetListOfCardsToSetAsUnallocatedAsync(null);
        }
        
        /// <remarks/>
        public void GetListOfCardsToSetAsUnallocatedAsync(object userState) {
            if ((this.GetListOfCardsToSetAsUnallocatedOperationCompleted == null)) {
                this.GetListOfCardsToSetAsUnallocatedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetListOfCardsToSetAsUnallocatedOperationCompleted);
            }
            this.InvokeAsync("GetListOfCardsToSetAsUnallocated", new object[0], this.GetListOfCardsToSetAsUnallocatedOperationCompleted, userState);
        }
        
        private void OnGetListOfCardsToSetAsUnallocatedOperationCompleted(object arg) {
            if ((this.GetListOfCardsToSetAsUnallocatedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetListOfCardsToSetAsUnallocatedCompleted(this, new GetListOfCardsToSetAsUnallocatedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetRequests", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SPRequest[] GetRequests(string ShortStoreName) {
            object[] results = this.Invoke("GetRequests", new object[] {
                        ShortStoreName});
            return ((SPRequest[])(results[0]));
        }
        
        /// <remarks/>
        public void GetRequestsAsync(string ShortStoreName) {
            this.GetRequestsAsync(ShortStoreName, null);
        }
        
        /// <remarks/>
        public void GetRequestsAsync(string ShortStoreName, object userState) {
            if ((this.GetRequestsOperationCompleted == null)) {
                this.GetRequestsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRequestsOperationCompleted);
            }
            this.InvokeAsync("GetRequests", new object[] {
                        ShortStoreName}, this.GetRequestsOperationCompleted, userState);
        }
        
        private void OnGetRequestsOperationCompleted(object arg) {
            if ((this.GetRequestsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRequestsCompleted(this, new GetRequestsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateRequest", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool UpdateRequest(SPRequest newRequest) {
            object[] results = this.Invoke("UpdateRequest", new object[] {
                        newRequest});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateRequestAsync(SPRequest newRequest) {
            this.UpdateRequestAsync(newRequest, null);
        }
        
        /// <remarks/>
        public void UpdateRequestAsync(SPRequest newRequest, object userState) {
            if ((this.UpdateRequestOperationCompleted == null)) {
                this.UpdateRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateRequestOperationCompleted);
            }
            this.InvokeAsync("UpdateRequest", new object[] {
                        newRequest}, this.UpdateRequestOperationCompleted, userState);
        }
        
        private void OnUpdateRequestOperationCompleted(object arg) {
            if ((this.UpdateRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateRequestCompleted(this, new UpdateRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetRecentCompleted", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SPResults GetRecentCompleted(string ShortStoreName) {
            object[] results = this.Invoke("GetRecentCompleted", new object[] {
                        ShortStoreName});
            return ((SPResults)(results[0]));
        }
        
        /// <remarks/>
        public void GetRecentCompletedAsync(string ShortStoreName) {
            this.GetRecentCompletedAsync(ShortStoreName, null);
        }
        
        /// <remarks/>
        public void GetRecentCompletedAsync(string ShortStoreName, object userState) {
            if ((this.GetRecentCompletedOperationCompleted == null)) {
                this.GetRecentCompletedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRecentCompletedOperationCompleted);
            }
            this.InvokeAsync("GetRecentCompleted", new object[] {
                        ShortStoreName}, this.GetRecentCompletedOperationCompleted, userState);
        }
        
        private void OnGetRecentCompletedOperationCompleted(object arg) {
            if ((this.GetRecentCompletedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRecentCompletedCompleted(this, new GetRecentCompletedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/InsertResults", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool InsertResults(SPResults newResults) {
            object[] results = this.Invoke("InsertResults", new object[] {
                        newResults});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void InsertResultsAsync(SPResults newResults) {
            this.InsertResultsAsync(newResults, null);
        }
        
        /// <remarks/>
        public void InsertResultsAsync(SPResults newResults, object userState) {
            if ((this.InsertResultsOperationCompleted == null)) {
                this.InsertResultsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInsertResultsOperationCompleted);
            }
            this.InvokeAsync("InsertResults", new object[] {
                        newResults}, this.InsertResultsOperationCompleted, userState);
        }
        
        private void OnInsertResultsOperationCompleted(object arg) {
            if ((this.InsertResultsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.InsertResultsCompleted(this, new InsertResultsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LastCompletedResultDate", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LastCompletedResultDate(string ShortStoreName) {
            object[] results = this.Invoke("LastCompletedResultDate", new object[] {
                        ShortStoreName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void LastCompletedResultDateAsync(string ShortStoreName) {
            this.LastCompletedResultDateAsync(ShortStoreName, null);
        }
        
        /// <remarks/>
        public void LastCompletedResultDateAsync(string ShortStoreName, object userState) {
            if ((this.LastCompletedResultDateOperationCompleted == null)) {
                this.LastCompletedResultDateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLastCompletedResultDateOperationCompleted);
            }
            this.InvokeAsync("LastCompletedResultDate", new object[] {
                        ShortStoreName}, this.LastCompletedResultDateOperationCompleted, userState);
        }
        
        private void OnLastCompletedResultDateOperationCompleted(object arg) {
            if ((this.LastCompletedResultDateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LastCompletedResultDateCompleted(this, new LastCompletedResultDateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Version {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SPResults {
        
        private int idField;
        
        private int storeIdField;
        
        private int requestIdField;
        
        private System.DateTime completedDateField;
        
        /// <remarks/>
        public int Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public int StoreId {
            get {
                return this.storeIdField;
            }
            set {
                this.storeIdField = value;
            }
        }
        
        /// <remarks/>
        public int RequestId {
            get {
                return this.requestIdField;
            }
            set {
                this.requestIdField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime CompletedDate {
            get {
                return this.completedDateField;
            }
            set {
                this.completedDateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SPRequest {
        
        private int idField;
        
        private int storeIdField;
        
        private SPRequestStatus statusField;
        
        private System.DateTime submitDateField;
        
        private SPSource sourceField;
        
        /// <remarks/>
        public int Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public int StoreId {
            get {
                return this.storeIdField;
            }
            set {
                this.storeIdField = value;
            }
        }
        
        /// <remarks/>
        public SPRequestStatus Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime SubmitDate {
            get {
                return this.submitDateField;
            }
            set {
                this.submitDateField = value;
            }
        }
        
        /// <remarks/>
        public SPSource Source {
            get {
                return this.sourceField;
            }
            set {
                this.sourceField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum SPRequestStatus {
        
        /// <remarks/>
        Requested,
        
        /// <remarks/>
        Pending,
        
        /// <remarks/>
        Errored,
        
        /// <remarks/>
        Complete,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum SPSource {
        
        /// <remarks/>
        Store,
        
        /// <remarks/>
        Website,
        
        /// <remarks/>
        Scheduled,
        
        /// <remarks/>
        Manual,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void GetTipshareVersionCompletedEventHandler(object sender, GetTipshareVersionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTipshareVersionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTipshareVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Version Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Version)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void GetProgramUsageCompletedEventHandler(object sender, GetProgramUsageCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetProgramUsageCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetProgramUsageCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void RecordProgramUsageCompletedEventHandler(object sender, RecordProgramUsageCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecordProgramUsageCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RecordProgramUsageCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void GetUndistributedStoresCompletedEventHandler(object sender, GetUndistributedStoresCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetUndistributedStoresCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetUndistributedStoresCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void GetListOfCardsToSetAsUnallocatedCompletedEventHandler(object sender, GetListOfCardsToSetAsUnallocatedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetListOfCardsToSetAsUnallocatedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetListOfCardsToSetAsUnallocatedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void GetRequestsCompletedEventHandler(object sender, GetRequestsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRequestsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRequestsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public SPRequest[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SPRequest[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void UpdateRequestCompletedEventHandler(object sender, UpdateRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void GetRecentCompletedCompletedEventHandler(object sender, GetRecentCompletedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRecentCompletedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRecentCompletedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public SPResults Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SPResults)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void InsertResultsCompletedEventHandler(object sender, InsertResultsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InsertResultsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal InsertResultsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void LastCompletedResultDateCompletedEventHandler(object sender, LastCompletedResultDateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LastCompletedResultDateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LastCompletedResultDateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591