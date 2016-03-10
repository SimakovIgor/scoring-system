﻿using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ScoringSystem {

    public partial class authorizationForm :Form {

        static int tryEnter = 3;
            
        public authorizationForm() {
            InitializeComponent();
        }

        private void authorizationForm_Load(object sender, EventArgs e) {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet.Role". При необходимости она может быть перемещена или удалена.
            this.roleTableAdapter.Fill(this.bankDataSet.Role);

        }

        private void exitButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void enterButton_Click(object sender, EventArgs e) {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Properties.Settings.Default.BankConnectionString;
            try {
                connection.Open();
                string role = accountComboBox.Text;
                string pass = passwordTextBox.Text;
                string checkPass = "";
                string sqlCommand = "select password from dbo.Role where role ='" + role + "'";
                SqlCommand cmd = new SqlCommand(sqlCommand, connection);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read()) {
                    checkPass = dr[0].ToString();
                }

                if (pass == checkPass && checkPass != "") {
                    currentClient.role = role;
                    this.Hide();
                    MainMenu mm = new MainMenu();
                    mm.Show();
                } else {
                    --tryEnter;
                    informationLabel.Text = "Неверный пароль, попробуйте еще раз!\nОсталось: " +
                        tryEnter + " попыток";
                    passwordTextBox.Text = "";
                    accountComboBox.Text = "";
                }
                if (tryEnter == 0) {
                    Application.Exit();
                }
            } catch {
                MessageBox.Show(string.Format("Ошибка в Базе Данных"));
            } finally {
                connection.Close();
            }
            

        }
    }
}
