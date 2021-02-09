<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Show_List.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container-fluid tm-container-content tm-mt-60">
<%--    <asp:DropDownList runat="server" ID="ddlLang" OnSelectedIndexChanged="ddlLang_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control col-xl-3 col-lg-4 col-md-6 col-sm-6 col-12 mb-5"></asp:DropDownList>
    <asp:LinkButton runat="server" ID="btnLang" OnClick="btnLang_Click" Text="تغيير اللغة"></asp:LinkButton>--%>
    </div>

    <div class="container-fluid tm-container-content tm-mt-60">
        <div class="row mb-4">
            <h2 class="col-6 tm-text-primary"><asp:Label runat="server" ID="lblShows">Latest Photos</asp:Label> 
            </h2>
            <div class="col-6 d-flex justify-content-end align-items-center">
                    Page
                    <input type="text" value="1" size="1" class="tm-input-paging tm-text-primary">
                    of 200
         
            </div>
        </div>

        <div class="row tm-mb-90 tm-gallery">

            <asp:Repeater runat="server" ID="rpShows" >
                <ItemTemplate>

            <div class="col-xl-3 col-lg-4 col-md-6 col-sm-6 col-12 mb-5">
                <figure class="effect-ming tm-video-item">
                    <img src='<%# Eval("Img_URL") %>' alt="Image" class="img-fluid">
                    <figcaption class="d-flex align-items-center justify-content-center">
                        <h2><%# Eval("Show_Name") %></h2>
                        <a href='<%# "Show_Page.aspx?AnimeID=" + Eval("Show_ID") %>'>View more</a>
                    </figcaption>
                </figure>
                <div class="d-flex justify-content-between tm-text-gray">
                    <span class="tm-text-gray-light"><%# Eval("Added_Date") %></span>
                    <span><%# Eval("Episodes") %> Episode</span>
                </div>
            </div>
                </ItemTemplate>
            </asp:Repeater>

            

          <%--  <div class="col-xl-3 col-lg-4 col-md-6 col-sm-6 col-12 mb-5">
                <figure class="effect-ming tm-video-item">
                    <img src="assets/img/img-03.jpg" alt="Image" class="img-fluid">
                    <figcaption class="d-flex align-items-center justify-content-center">
                        <h2>Clocks</h2>
                        <a href="photo-detail.html">View more</a>
                    </figcaption>
                </figure>
                <div class="d-flex justify-content-between tm-text-gray">
                    <span class="tm-text-gray-light">18 Oct 2020</span>
                    <span>9,906 views</span>
                </div>
            </div>--%>

        </div>
        <!-- row -->
        <div class="row tm-mb-90">
            <div class="col-12 d-flex justify-content-between align-items-center tm-paging-col">
                <a href="javascript:void(0);" class="btn btn-primary tm-btn-prev mb-2 disabled">Previous</a>
                <div class="tm-paging d-flex">
                    <a href="javascript:void(0);" class="active tm-paging-link">1</a>
                    <a href="javascript:void(0);" class="tm-paging-link">2</a>
                    <a href="javascript:void(0);" class="tm-paging-link">3</a>
                    <a href="javascript:void(0);" class="tm-paging-link">4</a>
                </div>
                <a href="javascript:void(0);" class="btn btn-primary tm-btn-next">Next Page</a>
            </div>
        </div>
    </div>
    <!-- container-fluid, tm-container-content -->



</asp:Content>
