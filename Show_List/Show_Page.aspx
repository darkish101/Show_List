<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Show_Page.aspx.cs" Inherits="Show_List.Show_Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="container-fluid tm-container-content tm-mt-60">
        <div class="row mb-4">
            <h2 runat="server" id="h2ShowTitle" class="col-12 tm-text-primary"></h2>
        </div>
        <div class="row tm-mb-90">
            <div class="col-xl-8 col-lg-7 col-md-6 col-sm-12">
                <img runat="server" id="imgShowImage" src="img/img-01-big.jpg" alt="Image" class="img-fluid">
            </div>
            <div class="col-xl-4 col-lg-5 col-md-6 col-sm-12">
                <div class="tm-bg-gray tm-video-details">
                 <%--   <p class="mb-4">
                        Please support us by making <a href="https://paypal.me/templatemo" target="_parent" rel="sponsored">a PayPal donation</a>. Nam ex nibh, efficitur eget libero ut, placerat aliquet justo. Cras nec varius leo.
                    </p>--%>
                    <div class="text-center mb-5">
                        <asp:Label runat="server" ID="lblEpisods" CssClass=" btn-primary tm-btn-big">2222</asp:Label>
                        <%--<a href="#" class="btn btn-primary tm-btn-big">Download</a>--%>
                    </div>
                    <div class="mb-4 d-flex flex-wrap">
                        <div class="mr-4 mb-2">
                          <span class="tm-text-gray-dark">All Episods: </span><span runat="server" id="sAllEpisods" class="tm-text-primary"></span>
                        </div>
                        <div class="mr-4 mb-2">
                            <span class="tm-text-gray-dark">Seen: </span><span runat="server" id="sSeen" class="tm-text-primary">1920x1080</span>
                        </div>
                    </div>
                    <div class="mb-4">
                        <h3 class="tm-text-gray-dark mb-3">Descripton</h3>
                        <p runat="server" id="pDescription"></p>
                    </div>
                    <div>
                        <h3 class="tm-text-gray-dark mb-3">Category</h3>
                        <asp:Repeater runat="server" ID="rpCategories">
                            <ItemTemplate>
<a href='<%# "Cat_Page.aspx?CatID=" + Eval("Cat_ID") %>' class="tm-text-primary mr-4 mb-2 d-inline-block"><%# Eval("Cat_Name") %></a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-4">
            <h2 class="col-12 tm-text-primary">Related Shows
            </h2>
        </div>
        <div class="row mb-3 tm-gallery">
           
            <div class="col-xl-3 col-lg-4 col-md-6 col-sm-6 col-12 mb-5">
                <figure class="effect-ming tm-video-item">
                    <img src="img/img-01.jpg" alt="Image" class="img-fluid">
                    <figcaption class="d-flex align-items-center justify-content-center">
                        <h2>Hangers</h2>
                        <a href="#">View more</a>
                    </figcaption>
                </figure>
                <div class="d-flex justify-content-between tm-text-gray">
                    <span class="tm-text-gray-light">16 Oct 2020</span>
                    <span>12,460 views</span>
                </div>
            </div>

        </div>
        <!-- row -->
    </div>
    <!-- container-fluid, tm-container-content -->
</asp:Content>
