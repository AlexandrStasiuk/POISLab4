using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POISLab4
{
    public partial class Schedule : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["CinemaDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadSchedule();
        }

        void LoadSchedule()
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT f.title, f.genre, s.start_time, s.hall
                               FROM sessions s JOIN films f ON s.film_id = f.id
                               ORDER BY s.start_time";
                var adapter = new NpgsqlDataAdapter(sql, conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void btnSaveXml_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                var ds = new DataSet("CinemaSchedule");

                var daFilms = new NpgsqlDataAdapter("SELECT * FROM films", conn);
                daFilms.Fill(ds, "Films");

                var daSessions = new NpgsqlDataAdapter("SELECT * FROM sessions", conn);
                daSessions.Fill(ds, "Sessions");

                ds.Relations.Add("Film_Sessions",
                    ds.Tables["Films"].Columns["id"],
                    ds.Tables["Sessions"].Columns["film_id"]);

                ds.WriteXml(Server.MapPath("~/App_Data/schedule.xml"), XmlWriteMode.WriteSchema);
            }

            lblMessage.Text = "Данные успешно сохранены в XML!";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                new NpgsqlCommand("DELETE FROM sessions", conn).ExecuteNonQuery();
                new NpgsqlCommand("DELETE FROM films", conn).ExecuteNonQuery();
                string sql = @"SELECT f.title, f.genre, s.start_time, s.hall
                               FROM sessions s JOIN films f ON s.film_id = f.id
                               ORDER BY s.start_time";
                var adapter = new NpgsqlDataAdapter(sql, conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
                lblMessage.Text = "Данные успешно удалены!";
            }
        }

        protected void btnLoadXml_Click(object sender, EventArgs e)
        {
            var ds = new DataSet();
            ds.ReadXml(Server.MapPath("~/App_Data/schedule.xml"));

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                new NpgsqlCommand("DELETE FROM sessions", conn).ExecuteNonQuery();
                new NpgsqlCommand("DELETE FROM films", conn).ExecuteNonQuery();

                if (!ds.Tables["Films"].Columns.Contains("new_id"))
                    ds.Tables["Films"].Columns.Add("new_id", typeof(int));

                foreach (DataRow film in ds.Tables["Films"].Rows)
                {
                    string title = film["title"].ToString();
                    string genre = film["genre"].ToString();
                    int duration = Convert.ToInt32(film["duration"]);

                    var cmdFilm = new NpgsqlCommand(
                        "INSERT INTO films (title, genre, duration) VALUES (@t, @g, @d) RETURNING id", conn);
                    cmdFilm.Parameters.AddWithValue("t", title);
                    cmdFilm.Parameters.AddWithValue("g", genre);
                    cmdFilm.Parameters.AddWithValue("d", duration);
                    int newFilmId = (int)cmdFilm.ExecuteScalar();

                    film["new_id"] = newFilmId;
                }

                foreach (DataRow session in ds.Tables["Sessions"].Rows)
                {
                    int oldFilmId = Convert.ToInt32(session["film_id"]);

                    DataRow filmRow = ds.Tables["Films"].Select("id = " + oldFilmId).FirstOrDefault();
                    if (filmRow == null) continue;

                    int newFilmId = Convert.ToInt32(filmRow["new_id"]);
                    DateTime start = DateTime.Parse(session["start_time"].ToString());
                    string hall = session["hall"].ToString();

                    var cmdSession = new NpgsqlCommand(
                        "INSERT INTO sessions (film_id, start_time, hall) VALUES (@f, @s, @h)", conn);
                    cmdSession.Parameters.AddWithValue("f", newFilmId);
                    cmdSession.Parameters.AddWithValue("s", start);
                    cmdSession.Parameters.AddWithValue("h", hall);
                    cmdSession.ExecuteNonQuery();
                }

                string sql = @"
            SELECT s.id, f.title AS film, f.genre, f.duration, s.start_time, s.hall
            FROM sessions s
            JOIN films f ON s.film_id = f.id
            ORDER BY s.start_time;";

                using (var adapter = new NpgsqlDataAdapter(sql, conn))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();
                }

            }

            lblMessage.Text = "Данные успешно загружены из XML!";
        }
    }
}