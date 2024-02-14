<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="m2_u1_w3_d3._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="container">
            <div class="row">
                <div class="col-4">
                    <div class="card first">
                        <div class="card-header">
                            <h2>Prenota il tuo Biglietto</h2>
                        </div>
                        <div class="card-body">
                            <div class="form-group mt-1">
                                <label for="nome">Nome:</label>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group mt-1">
                                <label for="cognome">Cognome:</label>
                                <asp:TextBox ID="txtCognome" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group mt-1">
                                <label for="sala">Sala:</label>
                                <asp:DropDownList ID="ddlSala" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mt-1">
                                <label for="tipoBiglietto">Tipo Biglietto:</label>
                                <asp:RadioButtonList ID="rblTipoBiglietto" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Intero" Value="Intero" Selected="True" />
                                    <asp:ListItem Text="Ridotto" Value="Ridotto" />
                                </asp:RadioButtonList>
                            </div>
                                <asp:Button ID="btnPrenota" runat="server" Text="Prenota" OnClick="btnPrenota_Click" CssClass="btn btn-dark text-light my-2" />
                            <div class="">
                                <asp:Literal ID="ltlMessaggio" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-4">
                    <div class="card all">
                        <div class="card-header">
                            <h2>Informazioni Sala</h2>
                        </div>
                        <div class="card-body">
                            <div class="form-group">
                                <label for="infoSala">Sala Selezionata:</label>
                                <asp:Label ID="lblSalaSelezionata" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="infoBigliettiVenduti">Biglietti Venduti:</label>
                                <asp:Label ID="lblBigliettiVenduti" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="infoBigliettiRidotti">Biglietti Ridotti Venduti:</label>
                                <asp:Label ID="lblBigliettiRidotti" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="infoPostiRimasti">Posti Rimasti:</label>
                                <asp:Label ID="lblPostiRimasti" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-4">
                    <div cssclass="button-group">
                        <asp:Button ID="btnMostraInfoSalaNORD" runat="server" Text="Mostra Info NORD" OnClick="btnMostraInfoSala_Click" CssClass="btn btn-dark text-light btn-sm mb-1" />
                        <asp:Button ID="btnMostraInfoSalaEST" runat="server" Text="Mostra Info EST" OnClick="btnMostraInfoSala_Click" CssClass="btn btn-dark text-light btn-sm mb-1" />
                        <asp:Button ID="btnMostraInfoSalaSUD" runat="server" Text="Mostra Info SUD" OnClick="btnMostraInfoSala_Click" CssClass="btn btn-dark text-light btn-sm mb-1" />
                    </div>
                    <asp:Panel ID="pnlDettagliSala" runat="server" Visible="false" CssClass="mt-3">
                        <div class="card all">
                            <div class="card-header">
                                <asp:Label ID="lblSala" runat="server" CssClass="header" />

                            </div>
                            <div class="card-body">
                                <asp:ListView ID="lvDettagliSala" runat="server">
                                    <ItemTemplate>
                                        <li><%# Container.DataItem %></li>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </main>

</asp:Content>
