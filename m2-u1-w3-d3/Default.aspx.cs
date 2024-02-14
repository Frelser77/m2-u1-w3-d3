using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using static m2_u1_w3_d3.DatabaseManager;

namespace m2_u1_w3_d3
{
    public partial class _Default : Page
    {
        public const int capienzaSala = 120;
        // Metodo che viene eseguito quando la pagina viene caricata per la prima volta
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DatabaseManager dbManager = new DatabaseManager();
                var sale = dbManager.GetSale(); //metodo restituisce la lista delle sale

                ddlSala.DataSource = sale;
                ddlSala.DataTextField = "NomeSala";
                ddlSala.DataValueField = "IdSala";
                ddlSala.DataBind();
            }
        }

        // Metodo per prenotare un biglietto per la sala selezionata 
        protected void btnPrenota_Click(object sender, EventArgs e)
        {
            // Ottenere i valori dai controlli della pagina
            string nome = txtNome.Text;
            string cognome = txtCognome.Text;

            // Controlla se il nome o il cognome sono vuoti
            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cognome))
            {
                ltlMessaggio.Text = "Per favore inserisci sia il nome che il cognome.";
                return;
            }

            DatabaseManager dbManager = new DatabaseManager();

            int idSala = int.Parse(ddlSala.SelectedValue);
            int postiOccupati = dbManager.GetConteggioBigliettiPerSala(idSala);
            const int capienzaSala = 120; // Capienza massima della sala

            if (postiOccupati >= capienzaSala)
            {
                ltlMessaggio.Text = "Spiacente, la sala è al completo.";
                return;
            }

            string tipoBiglietto = rblTipoBiglietto.SelectedValue;
            DateTime dataOra = DateTime.Now;

            // Prenotare il biglietto
            dbManager.PrenotaBiglietto(nome, cognome, tipoBiglietto, idSala, dataOra);


            postiOccupati = dbManager.GetConteggioBigliettiPerSala(idSala); // Aggiorna i posti occupati dopo l'inserimento
            int conteggioRidotti = dbManager.GetConteggioBigliettiRidottiPerSala(idSala);
            int postiRimasti = capienzaSala - postiOccupati;

            lblSalaSelezionata.Text = ddlSala.SelectedItem.Text;
            lblBigliettiVenduti.Text = postiOccupati.ToString();
            lblBigliettiRidotti.Text = conteggioRidotti.ToString();
            lblPostiRimasti.Text = postiRimasti.ToString();

            ltlMessaggio.Text = $"Biglietto prenotato con successo! Posti rimanenti {lblSalaSelezionata.Text} : {postiRimasti}.";
        }


        // Metodo per visualizzare le prenotazioni per la sala selezionata al click di un bottone 
        protected void btnMostraInfoSala_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int idSala;

            switch (btn.ID)
            {
                case "btnMostraInfoSalaNORD":
                    idSala = 1;
                    break;
                case "btnMostraInfoSalaEST":
                    idSala = 2;
                    break;
                case "btnMostraInfoSalaSUD":
                    idSala = 3;
                    break;
                default:
                    throw new Exception("ID del pulsante non riconosciuto.");
            }

            DatabaseManager dbManager = new DatabaseManager();
            SalaConPrenotazioni dettagliSala = dbManager.GetPrenotazioniPerSala(idSala);

            // Imposta il nome della sala
            lblSala.Text = dettagliSala.NomeSala;

            // Se la sala è libera o piena, crea una lista con un solo elemento
            if (dettagliSala.Prenotazioni.Count == 0)
            {
                lvDettagliSala.DataSource = new List<string> { "Sala libera" };
            }
            else if (dettagliSala.Prenotazioni.Count >= capienzaSala)
            {
                lvDettagliSala.DataSource = new List<string> { "Sala piena" };
            }
            else
            {
                // Crea una lista di stringhe con i dettagli delle prenotazioni
                List<string> dettagliPrenotazioni = new List<string>();
                foreach (DatabaseManager.Prenotazione prenotazione in dettagliSala.Prenotazioni)
                {
                    string dettaglioPrenotazione = $"Nome: {prenotazione.Nome}, Cognome: {prenotazione.Cognome}, Data e ora: {prenotazione.DataOra}";
                    dettagliPrenotazioni.Add(dettaglioPrenotazione);
                }
                lvDettagliSala.DataSource = dettagliPrenotazioni;
            }

            // Esegue il binding dei dati
            lvDettagliSala.DataBind();

            // Rende visibile il pannello pnlDettagliSala
            pnlDettagliSala.Visible = true;
        }
    }

}

