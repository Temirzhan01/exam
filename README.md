            data = $("#RequestedConditions_" + i + "__IsCommissionsRequested").is(":checked")
            $.get(urlBase + "CheckService/IsCommissionDocExist", data, function (result)
            {
                alert(result)
            });

                    public Tuple<bool, string> IsCommissionDocExist(bool isChecked)
        {
            BaseViewModel bv = _BaseViewModel;
            using (uploaderView.UploaderViewWS atStorService = new uploaderView.UploaderViewWS() { UseDefaultCredentials = true })
            {
                if (isChecked && !atStorService.GetViewPage(ConfigurationManager.AppSettings["CredManagerViewID"].ToString(), bv.RequestNumber).Files.Any(a => a.DocsViewDocumentID == ConfigurationManager.AppSettings["CommissionDocViewId"]))
                {
                    return new Tuple<bool, string>(false, "Необходимо вложить подтверждающий \"Квитанция об уплате комиссии за рассмотрение заявки\"");
                }
                else if (!isChecked && !atStorService.GetViewPage(ConfigurationManager.AppSettings["CredManagerViewID"].ToString(), bv.RequestNumber).Files.Any(a => a.DocsViewDocumentID == ConfigurationManager.AppSettings["CommissionUnnecessaryDocViewId"]))
                {
                    return new Tuple<bool, string>(false, "Необходимо вложить \"Решения УО об отмене комиссии\"");
                }
                return new Tuple<bool, string>(true, "");
            }
        }
