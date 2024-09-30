                                    @using (var docs = ko.Foreach(m => m.MDocuments))
                                    {
                                    <tr>
                                        @using (docs.If(m => m.isUpload))
                                        {
                                            using (var doc = docs.With(m => m.document))
                                            {
                                                <td>
                                                    @doc.Html.Span(m => m.Name)
                                                </td>
                                                <td>
                                                    <div class="DownloadPart">
                                                        <input type="button" value="Просмотр" class="btn btn-xs btn-success" data-bind="click: function() { downloadFile($index()); }" />
                                                        @ko.Html.Button("Удалить", "DeleteFile", "Base", new { index = docs.GetIndex() }, new { @class = "btn btn-xs btn-danger" })
                                                    </div>
                                                    <div class="col-xs-12 docInfo">
                                                        <div class="row">
                                                            <div class="col-xs-6">
                                                                @doc.Html.KoTextBox(m => m.docNum, "Номер документа", "", false, "text", "", "required")
                                                            </div>
                                                            <div class="col-xs-6">
                                                                <span style="display:none" data-bind="text: $index()"></span>
                                                                @doc.Html.KoTextBox(m => m.docDate, "Дата документа", "js__date", false, "text", "", "required datetimepicker datetimepickerDOC")
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>


                                            }
                                        }
                                        @using (docs.If(m => (!m.isUpload)))
                                        {
                                            using (var doc = docs.With(m => m.document))
                                            {
                                                using (doc.If(m => m.ID == 9))
                                                {
                                                    <td>
                                                        @doc.Html.Span(m => m.Name)
                                                    </td>

                                                    <td>
                                                        <div class="UploadPart">
                                                            <div class="btn btn-xs btn-primary pull-left btn-file">
                                                                Вложить документ <input name="upload" type="file" class="required" id="documentUpload" data-bind="event: { change: function() { onFilesSelected($element.files[0], $index());   } }, uniqueName : true" accept="*/*" />
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        @ko.Html.Button("-------", "DeleteDocRow", "Base", new { index = docs.GetIndex() }, new { @class = "btn btn-xs btn-danger" })
                                                    </td>
                                                }
                                            }
                                        }
                                        </tr>
                                    }
