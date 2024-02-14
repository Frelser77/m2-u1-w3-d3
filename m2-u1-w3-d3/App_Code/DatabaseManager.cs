using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace m2_u1_w3_d3
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["CinemaDBConnectionString"].ConnectionString;
        }

        public class Sala
        {
            public int IdSala { get; set; }
            public string NomeSala { get; set; }
        }

        public class Prenotazione
        {
            public string Nome { get; set; }
            public string Cognome { get; set; }
            public DateTime DataOra { get; set; }
        }

        public class SalaConPrenotazioni
        {
            public string NomeSala { get; set; }
            public List<Prenotazione> Prenotazioni { get; set; }
        }

        // Metodo che apre la connessione e esegue il codice SQL
        public void ApriConnessioneEseguiAzione(string nome, string cognome, string tipoBiglietto, int idSala, DateTime dataOra)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // La connessione è stata correttamente aperta
                    InserisciBiglietto(conn, nome, cognome, tipoBiglietto, idSala, dataOra);
                }
                catch (SqlException ex)
                {
                    // La connessione non può essere aperta o è avvenuta un'eccezione
                    throw new Exception("Errore durante la connessione al database: " + ex);
                }
            }
        }


        private void InserisciBiglietto(SqlConnection conn, string nome, string cognome, string tipoBiglietto, int idSala, DateTime dataOra)
        {
            string query = "INSERT INTO Biglietti (Nome, Cognome, TipoBiglietto, IdSala, DataOra) VALUES (@Nome, @Cognome, @TipoBiglietto, @IdSala, @DataOra)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Cognome", cognome);
                cmd.Parameters.AddWithValue("@TipoBiglietto", tipoBiglietto);
                cmd.Parameters.AddWithValue("@IdSala", idSala);
                cmd.Parameters.AddWithValue("@DataOra", dataOra);

                cmd.ExecuteNonQuery();
            }
        }

        public void PrenotaBiglietto(string nome, string cognome, string tipoBiglietto, int idSala, DateTime dataOra)
        {
            ApriConnessioneEseguiAzione(nome, cognome, tipoBiglietto, idSala, dataOra);
        }

        public List<Sala> GetSale()
        {
            List<Sala> sale = new List<Sala>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT IdSala, NomeSala FROM Sale";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sale.Add(new Sala
                                {
                                    IdSala = reader.GetInt32(reader.GetOrdinal("IdSala")),
                                    NomeSala = reader.GetString(reader.GetOrdinal("NomeSala"))
                                });
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Gestisce l'eccezione
                    throw new Exception("Errore durante il recupero delle sale: " + ex);
                }
            }

            return sale;
        }

        public int GetConteggioBigliettiPerSala(int idSala)
        {
            int conteggio = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Biglietti WHERE IdSala = @IdSala";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdSala", idSala);
                    conteggio = (int)cmd.ExecuteScalar();
                }
            }
            return conteggio;
        }

        public int GetConteggioBigliettiRidottiPerSala(int idSala)
        {
            int conteggioRidotti = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Biglietti WHERE IdSala = @IdSala AND TipoBiglietto = 'Ridotto'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdSala", idSala);
                    conteggioRidotti = (int)cmd.ExecuteScalar();
                }
            }
            return conteggioRidotti;
        }

        // metodo per visualizzare le prenotazioni per la sala selezionata al click di un bottone
        public SalaConPrenotazioni GetPrenotazioniPerSala(int idSala)
        {
            SalaConPrenotazioni salaConPrenotazioni = new SalaConPrenotazioni();
            salaConPrenotazioni.Prenotazioni = new List<Prenotazione>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT B.Nome, B.Cognome, B.DataOra, S.NomeSala FROM Biglietti AS B LEFT JOIN Sale AS S ON B.IdSala = S.IdSala WHERE S.IdSala = @IdSala";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdSala", idSala);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (salaConPrenotazioni.NomeSala == null)
                            {
                                salaConPrenotazioni.NomeSala = reader["NomeSala"].ToString();
                            }

                            salaConPrenotazioni.Prenotazioni.Add(new Prenotazione
                            {
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                DataOra = reader.GetDateTime(reader.GetOrdinal("DataOra"))
                            });
                        }
                    }
                }
            }

            return salaConPrenotazioni;
        }
    }
}
