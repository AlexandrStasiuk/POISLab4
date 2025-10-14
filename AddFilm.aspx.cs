using System;
using Npgsql;

namespace POISLab4
{
    public partial class AddFilm : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["CinemaDb"].ConnectionString;

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "INSERT INTO films (title, genre, duration) VALUES (@t, @g, @d)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("t", txtTitle.Text);
                    cmd.Parameters.AddWithValue("g", txtGenre.Text);
                    cmd.Parameters.AddWithValue("d", int.Parse(txtDuration.Text));
                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "Фильм успешно добавлен!";
        }
    }
}