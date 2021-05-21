using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManager
{
    class DBHelper
    {
        public static SqlConnection conn = new SqlConnection();     //접속해주는 칭구
        public static SqlDataAdapter da;
        public static DataSet ds;       //메모리 상의 DB를 표현하는 개체
        public static DataTable dt;     //메모리 상의 하나의 테이블을 표현하는 개체
        // DataSet 개체를 생성한 후에 DB를 구성할 테이블과 관계를 정의할 수 있다.

        public static void ConnectDB()
        {
            //DB 접속 정보
            conn.ConnectionString = string.Format("Data Source=({0}); " +
                "Initial Catalog = {1};" +
                "Integrated Security = {2};" +
                "Timeout = 3"
                , "local", "MYDB1", "SSPI");
            conn = new SqlConnection(conn.ConnectionString);
            conn.Open();
        }

        public static void selectQuery(int parkingSpot = -1)
        {
            ConnectDB();

            //Sql 명령어 선언
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            if (parkingSpot < 0)
            {
                cmd.CommandText = "SELECT * FROM CarManager";

                //DataAdapter와 DataSet으로 DB table 불러오기
                //DataSet은 메모리상의 하나의 DB객체이며 DataTable은 메모리상의 하나의 테이블 객체
                da = new SqlDataAdapter(cmd);   //select구문 들어감
                ds = new DataSet();
                da.Fill(ds, "CarManager");

                conn.Close();
            }
            else
            {
                cmd.CommandText = "SELECT * FROM CarManager WHERE ParkingSpot = " + parkingSpot;

                da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "CarManager");

                dt = ds.Tables[0];  //select 결과값을 DataTable에 넣는다
                conn.Close();
            }    
        }
        
        public static void insertQuery(int parkingSpot)
        {
            try
            {
                ConnectDB();
                string sqlcommand = "INSERT INTO CarManager (ParkingSpot_ VALUES (@parameter1)";
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                //Column 명은 별도의 파라메터 형태로 선언함
                //SQL Injection 방지하고자함(유효하지 않은 데이터를 이용한 공격 예: +나 ' 기호를 이용한 공격)
                cmd.Parameters.AddWithValue("@parameter1", parkingSpot);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        //출차인 경우에는 true 값을 넣어줌
        //선택적 변수(값을 넣을지 말지 선택)는 !반드시! 맨 마지막 파라메터에 위치해야함

        public static void updateQuery(string parkingSpotText, string carNumberText, string driverNameText, string phoneNumber, bool isRemove = false)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                string sqlcommand;
                if (isRemove)
                {
                    sqlcommand = "Update CarManager set CarNumber = '', DriverName = '', PhoneNumber = '', ParkingTime = null where ParkingSpot = @p1";

                    cmd.Parameters.AddWithValue("@p1", parkingSpotText);
                }
                else
                {
                    sqlcommand = "Update CarManager set CarNumber = @p1, DriverName = @p2, PhoneNumber = @p3, ParkingTime = @p4 where ParkingSpot = @p5";

                    cmd.Parameters.AddWithValue("@p1", carNumberText);
                    cmd.Parameters.AddWithValue("@p2", driverNameText);
                    cmd.Parameters.AddWithValue("@p3", phoneNumber);
                    cmd.Parameters.AddWithValue("@p4", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@p5", parkingSpotText); //제대로 된 숫자 입력하지 않으면 이 부분까지 코드가 실행되지도 못 하도록 짜여있음
                }
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery(); //쿼리 실행
                conn.Close();
            }
            catch (Exception e)
            {

                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

    }
}
