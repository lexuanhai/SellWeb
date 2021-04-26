(function ($) {
    var self = this;
    self.Product = {
        Id: null,
        CategoryId: null,
        Head: null,
        Description: "",
        Price: 0,
        PromotionalPrice: 0,
        Views: 0,
        Status: null,
        ManufactureDate: "",
        ExpirationDate: ""
    }

    self.lstProductImages = [];

    self.Colors = [];

    self.Sizes = [];

    self.Color = {
        Id: null,
        Code: "",
        Name:""
    }

    self.Quantities = {
        ColorId: null,
        ProductId: null,
        TotalImport: 0,
        TotalSell: 0,
        TotalStock: 0
    }

    self.Files = [];

    self.Categories = [];

    self.addSerialNumber = function (classTr) {
        var index = 0;
        $(classTr).each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };

    self.Init = function () {

        CKEDITOR.replace('titleProduct');

        CKEDITOR.replace('content');
       
        $("#images").change(function () {          
            var files = $(this).prop('files');
            $.each(files, function (index, file) {
                var t = file.type.split('/').pop().toLowerCase();

                if (t != "jpeg" && t != "jpg" && t != "png" && t != "bmp" && t != "gif") {
                    alert('Vui lòng chọn một tập tin hình ảnh hợp lệ!');
                    return false;
                }

                if (file.size > 2048000) {
                    alert('Kích thước tải lên tối đa chỉ 2Mb');
                    return false;
                }
                var img = new Image();
                img.src = URL.createObjectURL(file);
                img.onload = function () {
                    CheckWidthHeight(this.width, this.height);
                }
                var CheckWidthHeight = function (w, h) {
                    if (w <= 300 && h <= 300) {
                        alert("Ảnh tối thiểu 300 x 300 px");
                    }
                    else {
                        $(".lst-img").append("<div class='item' style='background: url(" + img.src + ")'><span class='remove-img fa fa-trash-o fa-lg' data-name='" + files.name + "'></span></div>");
                        self.Files.push(file);
                    }
                }
            });
            setTimeout(function () {
                self.ParsingUrlImages();
            }, 1000);
        })

        $('body').on('click', '.remove-img', function () {
            var nameimg = $(this).attr('data-path');
            console.log(self.Files);
            console.log(nameimg);
            //var statusRemove = false;
            //$.each(self.Files, function (key, item) {
            //    if (item !== undefined) {
            //        if (item.name === nameimg) {
            //            var index = self.Files.indexOf(item);
            //            if (index !== -1) {
            //                self.Files.splice(index, 1);
            //                statusRemove = true;
            //            }
            //            else {
            //                statusRemove = false;
            //            }
            //        }
            //    }             
            //});
            //if (statusRemove == true) {
            //    $(this).parent().remove();
            //}
        })

        $(".btn-add").click(function () {
            $(".product-content").hide();
            $(".product-add-edit").show();
        })

        $(".btn-add-quantity").click(function () {
            var html = '<tr class="number-row-quantities"><td></td>' +
                '<td><input type="text" class="form-control quantity"></td>' +
                '<td>' + self.BindHtmlSize(0) + '</td>' +
                '<td>' + self.BindHtmlColor(0) + '</td>' +
                            '<td>' +
                                '<a class="btn-edit-quantity fa fa-pencil-square fa-lg" title="Sửa" data-id="2" href="javascript:void(0)"></a>'+
                                '<a class="btn-delete-quantity fa fa-trash-o fa-lg" data-id="2" data-fullname="Lê Xuân Hải" title="xóa" href="javascript:void(0)"></a>'+
                            '</td > '+
                        '</tr >';        
        
            $(".tb-quantity tbody").append(html);
        
            self.addSerialNumber(".number-row-quantities");
        })

        $('body').on('click', '.btn-delete-quantity', function () {
            $(this).closest('tr').remove();            
        })

        $('body').on('click', '.btn-edit-quantity', function () {
            $(this).closest('tr').find(".quantity").first().focus();
            $(this).closest('tr').find(".size").first().focus();
            //$(this).closest('tr').find(".color").focus();
        })

        $('body').on('click', '.btn-edit', function () {

            var productId = $(this).attr('data-id');

            if (productId > 0) {

                self.GetProductById(productId);

                self.GetImagesByProductId(productId);

                self.GetQuantitiesByProductId(productId);

                $(".product-content").hide();

                $(".product-add-edit").show();
            }            
        })

        $(".date-format").datepicker({
            changeMonth: true,
            changeYear: true,
        });

        $(".btn-save").click(function () {
            self.GetData();
            self.SubmitForm();     
        })    

        $('body').on('click', '.remove-img', function () {
            debugger;
            var nameimg = $(this).attr('data-path');
            if (nameimg !== null && nameimg !== '') {
                var indexImg = self.Files.indexOf(nameimg);
                if (indexImg >= 0) {
                    self.Files.splice(indexImg, 1);
                    $(this).closest("div").remove();
                    console.log(self.Files);
                }
            }
           
        })

        self.GetColor();

        self.GetSize();

        self.GetProductPaging();

        self.GetCategory();

        $("#categoryId").change(function () {
            self.Product.CategoryId = parseInt($(this).val());            
            if (self.Product.CategoryId > 0) {
                self.BindSubCategory(self.Product.CategoryId);
            }
        })

        $("#subCategoryId").change(function () {
            var subCategoryId = parseInt($(this).val());
            if (subCategoryId > 0) {
                self.Product.CategoryId = subCategoryId;
            }
        })

        $(".price").on('keyup', function () {
            self.AddCommas($(this));
        })
        
    }
    self.GetCategory = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/ProductCategory/GetAllCategory",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    self.Categories = response.Data;
                    self.BindHtmlParentCategory();
                }
            },
            error: function () {
            }
        });
    }

    self.BindHtmlParentCategory = function () {
        if (self.Categories !== null && self.Categories.length > 0) {
            var html = '<option value="0">Loại sản phẩm</option>';            
            $.each(self.Categories, function (key, item) {
                if (item.ParentId == null) {
                    html += '<option value="' + item.Id + '">' + item.Name + '</option>'
                }              
            })
            $("#categoryParentId").html(html);
        }
    }

    self.GetProductById = function (productId) {
        $.ajax({
            type: "GET",
            url: "/Admin/Product/GetProductById",
            data: {
                productId: productId
            },
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var product = response.Data;
                    self.Product.Id = product.Id;
                    self.Product.Name = product.Name;
                    self.Product.ProductCategoryId = product.CategoryId;
                    self.Product.Price = product.Price;
                    self.Product.PromotionalPrice = product.PromotionalPrice;
                    self.Product.Head = product.Head;
                    self.Product.Description = product.Description;
                    self.Product.ManufactureDate = product.ManufactureDate;
                    self.Product.ExpirationDate = product.ExpirationDate;
                    self.Product.CreatedDate = product.CreatedDate;
                    self.SetData();
                }
            },
            error: function () {
            }
        });
    }

    self.SetData = function () {
        $("#name").val(self.Product.Name);
        $("#price").val(self.Product.Price.toLocaleString());
        if (self.Product.ProductCategoryId > 0) {
            var category = self.Categories.find(c => c.Id == self.Product.ProductCategoryId);            
            if (category.ParentId !== null && category.ParentId > 0) {
                self.BindSubCategory(category.ParentId);
                $("#subCategoryId").val(category.Id);
            }
            else {
                $("#subCategoryId").val(0);  
            }
            $("#categoryParentId").val(category.Id);
        }
        console.log(self.Product.ProductCategoryId);
        //$("#subCategoryId").val(self.Product.ProductCategoryId !== null ? self.Product.ProductCategoryId: 0);
        $("#promotionalPrice").val(self.Product.PromotionalPrice.toLocaleString());
        $("#manufactureDate").val(self.Product.ManufactureDate !== null && self.Product.ManufactureDate !== '' ? dateFormatJson(self.Product.ManufactureDate) : "");
        $("#expirationDate").val(self.Product.ExpirationDate !== null && self.Product.ExpirationDate !==''? dateFormatJson(self.Product.ExpirationDate): "");
        CKEDITOR.instances.titleProduct.setData(self.Product.Head);
        CKEDITOR.instances.content.setData(self.Product.Description);
    }

    self.AddCommas = function (input) {
        var num = self.GetNumber(input.val());
        if (num == 0) {
            input.val('');
        } else {
            input.val(num.toLocaleString());
        }
    }

    self.GetNumber = function (str) {
        var arr = str.split('');
        var out = new Array();
        for (var cnt = 0; cnt < arr.length; cnt++) {
            if (isNaN(arr[cnt]) == false) {
                out.push(arr[cnt]);
            }
        }
        return Number(out.join(''));
    }

    self.GetImagesByProductId = function (productId) {
        $.ajax({
            type: "GET",
            url: "/Admin/ProductImages/GetProductImagesById",
            data: {
                productId: productId
            },
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var html = "";                  
                    $.each(response.Data, function (key, item) {
                        self.Files.push(item.Url);
                        html += '<div class="item" style = "background: url(' + item.Url + ')" > <span class="remove-img fa fa-trash-o fa-lg" data-path =' + item.Url + '></span></div >';
                    })
                    $(".lst-img").append(html);
                }
            },
            error: function () {
            }
        });
    }

    self.GetQuantitiesByProductId = function (productId) {
        $.ajax({
            type: "GET",
            url: "/Admin/ProductQuantity/GetQuantitiesByProductId",
            data: {
                productId: productId
            },
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    self.Quantities = response.Data;
                    if (self.Quantities.length > 0) {
                        var html = '';
                        $.each(self.Quantities, function (key, item) {
                            html += '<tr class="number-row-quantities"><td></td>' +
                                '<td><input type="text" class="form-control quantity" value=' + item.TotalImport +'></td>' +
                                '<td>' + self.BindHtmlSize(item.SizeId) + '</td>' +
                                '<td>' + self.BindHtmlColor(item.ColorId) + '</td>' +
                                '<td>' +
                                '<a class="btn-edit-quantity fa fa-pencil-square fa-lg" title="Sửa" data-id="2" href="javascript:void(0)"></a>' +
                                '<a class="btn-delete-quantity fa fa-trash-o fa-lg" data-id="2" data-fullname="Lê Xuân Hải" title="xóa" href="javascript:void(0)"></a>' +
                                '</td>' +
                                '</tr>';      
                        })

                        $(".tb-quantity tbody").append(html);

                        self.addSerialNumber(".number-row-quantities");
                    }
                    
                }
            },
            error: function () {
            }
        });
    }
    
    self.BindSubCategory = function (categoryId) {
        if (categoryId > 0) {
            var html = '<option value="0">Danh mục con</option>';         
            $.each(self.Categories, function (key, item) {
                if (item.ParentId == categoryId) {
                    html += '<option value="' + item.Id + '">' + item.Name + '</option>'
                }
            })
            $("#subCategoryId").html(html);
        }
    }

    self.GetColor = function () {        
        $.ajax({
            type: "GET",
            url: "/Admin/Color/GetAll",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var html = '<option value="0">Màu sắc</option>';
                    $.each(response.Data, function (key, item) {
                        self.Colors.push(item);         
                        html += '<option value="' + item.Id + '">' + item.Name + '</option>'
                    })
                    $(".color").html(html);
                }
            },
            error: function () {
            }
        });
    }

    self.GetSize = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/Size/GetAll",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var html = '<option value="0">Kích thước</option>';
                    $.each(response.Data, function (key, item) {
                        self.Sizes.push(item);
                        html += '<option value="' + item.Id + '">' + item.Name + '</option>'
                    })
                    $(".size").html(html);
                }
            },
            error: function () {
            }
        });
    }
    // 0 Lấy tất cả
    self.BindHtmlColor = function (colorId) {
        var html = '<select class="form-control color"> <option value="0">Màu sắc</option>';
        $.each(self.Colors, function (key, item) {
            if (item.Id == colorId) {
                html += '<option value = "' + item.Id + '">' + item.Name + '</option>';
            }
            else {
                html += '<option value = "' + item.Id + '">' + item.Name + '</option>';
            }            
        })
        html += '</select>';
        return html;
    }

    self.BindHtmlSize = function (sizeId) {
        var html = '<select class="form-control size"><option value="0">Kích thước</option>';
        $.each(self.Sizes, function (key, item) {
            if (item.Id == sizeId) {
                html += '<option value = "' + item.Id + '">' + item.Name + '</option>';
            }
            else {
                html += '<option value = "' + item.Id + '">' + item.Name + '</option>';
            }
        })

        html += '</select>';

        return html;
    }

    self.GetData = function () {
        self.Product.Name = $("#name").val();
        self.Product.Price = $("#price").val();
        self.Product.PromotionalPrice = $("#promotionalPrice").val();
        self.Product.ManufactureDate = $("#manufactureDate").val();
        self.Product.ExpirationDate = $("#expirationDate").val();
        self.Product.Head = CKEDITOR.instances.titleProduct.getData();
        self.Product.Description = CKEDITOR.instances.content.getData();
    }

    self.SaveProduct = function () {
        $.ajax({
            type: "POST",
            url: "/Admin/Product/SaveEntity",
            data: self.Product,
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
                $(".add-edit-user").css("display", "inline-block");
            },
            success: function (response) {   
                if (response.Status == true) {
                    self.Product.Id = response.ProductId;
                    if (self.Product.Id > 0) {                        
                        self.SaveProductQuantity();
                        if (self.Files.length > 0) {                            
                            self.SaveProductImages(self.lstProductImages, self.Product.Id);
                         }
                    }
                }
                else {
                    $.notify(response.Message, 'error');
                }
            },
            error: function (eror) {
                console.log(eror);
            }
        });
    }

    self.ParsingUrlImages = function () {
        var data = new FormData();        
        $.each(self.Files, function (key, item) {
            data.append(item.name, item);
        })
        $.ajax({
            type: "POST",
            url: "/Admin/ProductImages/UploadImage",
            contentType: false,
            processData: false,
            data: data,
            success: function (res) {
                if (res !== null && res.length > 0) {
                    self.lstProductImages = res;  
                }
            },
            error: function () {
            }
        });
    }

    self.SaveProductImages = function (lstImages,productId) {
        $.ajax({
            type: "POST",
            url: "/Admin/ProductImages/SaveEntity",
            data: {
                productImages: lstImages,
                productId: productId
            },
            beforeSend: function () {
            },
            complete: function () {
                $(".add-edit-user").css("display", "inline-block");
            },
            success: function (response) {
                if (response.Status == true) {
                }
                else {
                }
            },
            error: function (eror) {
                console.log(eror);
            }
        });
    }

    self.SaveProductQuantity = function () {
        var lstProductQuantity = [];
        $.each($('.tb-quantity tbody tr'), function (i, item) {
            lstProductQuantity.push({
                ProductId: self.Product.Id,
                TotalImport: parseInt($(item).find(".quantity").val()),
                SizeId: parseInt($(item).find(".size").val()),
                ColorId: parseInt($(item).find(".color").val()),
            });
        });
        if (lstProductQuantity.length > 0) {
            $.ajax({
                type: "POST",
                url: "/Admin/ProductQuantity/SaveEntity",
                data: {
                    productQuantity: lstProductQuantity
                },
                beforeSend: function () {
                },
                complete: function () {
                    $(".add-edit-user").css("display", "inline-block");
                },
                success: function (response) {
                    if (response.Status == true) {
                        //self.GetProductCateogory();
                        //$('#create').modal('hide');
                        $.notify(response.Message, 'success');
                        //self.GetParentProductCategory();
                    }
                    else {
                        $.notify(response.Message, 'error');
                    }
                },
                error: function (eror) {
                    console.log(eror);
                }
            });
        }

    }

    self.SubmitForm = function () {
       // CKEDITOR.instances.content.updateElement();
        $("#form_submit").validate({
            //ignore: [],
            rules:
            {
                //name: {
                //    required: true,
                //},
                //price: {
                //    required: true,
                //},
                //manufactureDate: {
                //    required: true,
                //},
                //promotionalPrice: {
                //    required: true,
                //}
                //titleProduct: {
                //    required: true,
                //},
                //content: {
                //    required: true                    
                //}
            },
            messages:
            {
                //name: {
                //    required: "Bạn chưa nhập tên sản phẩm.",
                //},
                //price: {
                //    required: "Bạn chưa giá.",
                //},
                //manufactureDate: {
                //    required: "Bạn chưa nhập ngày sản xuất",
                //},
                //promotionalPrice: {
                //    required: "Bạn chưa nhập ngày hết hạn",
                //}
                //titleProduct: {
                //    required: "Bạn chưa nhập tiêu đề",
                //},
                //content: {
                //    required: "Bạn chưa nhập môt tả",
                //}
            },
            submitHandler: function (form) {
                var ckTitleProduct = CKEDITOR.instances.titleProduct.getData();
                var ckDescription = CKEDITOR.instances.content.getData();
                if (ckTitleProduct == "") {
                    alert("Bạn chưa nhập tiêu đề sản phẩm");
                    return false;
                }
                if (ckDescription =="") {
                    alert("Bạn chưa nhập nội dung sản phẩm");
                    return false;
                }
                self.GetData();
                self.SaveProduct();
                //self.submitFunction(self.ProductCategory);
            }
        });
    }

    self.GetProductPaging = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/Product/GetPaging",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var data = response.Results;
                var html = "";
                if (data !== null && data.length >0) {
                    var html = '';
                    $.each(data, function (key, item) {                       
                        html += "<tr class='number-row-product'>" +
                            "<td></td>" +
                            "<td>" + item.Name + "</td>" +
                            "<td>" + (item.ProductCategoryParent !== null ? item.ProductCategoryParent.Name : "") + "</td>" +
                            "<td>" + (item.ProductCategories !== null ? item.ProductCategories.Name:"") + "</td>" +
                            "<td>" + (item.Price !== null ? item.Price.toLocaleString() : "") + "</td>" +
                            "<td>" + (item.PromotionalPrice !== null ? item.PromotionalPrice.toLocaleString() : "") + "</td>" +
                            "<td>" + (item.ManufactureDate !== null ? dateFormatJson(item.ManufactureDate) : "") + "</td>" +
                            "<td>" + (item.ExpirationDate !== null ? dateFormatJson(item.ExpirationDate) : "") + "</td>" +
                            "<td>" + (item.CreatedDate !== null ? dateFormatJson(item.CreatedDate) : "") + "</td>" +
                            '<td>'+
                            '<a class="btn-edit fa fa-pencil-square fa-lg" title = "Sửa" data-id=' + item.Id + ' data-id=' + item.Id + '  href="javascript:void(0)" ></a >' +
                            '<a class="btn-delete fa fa-trash-o fa-lg red" data-id=' + item.Id + ' data-name ="' + item.Name + '" title="xóa" href="javascript:void(0)"></a>' +
                            '</td >' +
                            "</tr>";
                    })
                    $(".table-data").html(html);
                    self.addSerialNumber('.number-row-product');
                }
            },
            error: function () {
            }
        });
    }

    $(document).ready(function () {
        self.Init();
    })
})(jQuery);