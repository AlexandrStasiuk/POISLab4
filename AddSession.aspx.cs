using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POISLab4
{
    public partial class AddSession : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["CinemaDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) LoadFilms();
        }

        void LoadFilms()
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("SELECT id, title FROM films", conn);
                var reader = cmd.ExecuteReader();
                ddlFilms.DataSource = reader;
                ddlFilms.DataValueField = "id";
                ddlFilms.DataTextField = "title";
                ddlFilms.DataBind();
            }
        }

        protected void btnAddSession_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                var sql = "INSERT INTO sessions (film_id, start_time, hall) VALUES (@f, @t, @h)";
                var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("f", int.Parse(ddlFilms.SelectedValue));
                cmd.Parameters.AddWithValue("t", DateTime.Parse(txtStart.Text));
                cmd.Parameters.AddWithValue("h", txtHall.Text);
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Сеанс успешно добавлен!";
        }
    }
}