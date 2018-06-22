using System;
using System.Web.Http;
using RMFFramework_CM.Models;
using System.Text.RegularExpressions;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RMFFramework_CM.Controllers
{
    public class MainController : ApiController
    {
        #region "INITIALIZATION"
        Users user = new Users();
        Areas area = new Areas();
        Branches branch = new Branches();
        Regions region = new Regions();
        Employees emp = new Employees ();
        #endregion

        [AcceptVerbs("GET","POST")]

        #region "HELPER METHODS"
        private string encrypt(string text)
        {
            string salt = "mW1RcV8d{Hu%nQ&!on0KT*xG-{I2a(+^AR9fE^J~n-k3h@w}*v{:GF*E&B2*xZ{+";
            string saltedTxt = String.Format("{1}{0}{1}", text, salt);
            byte[] byteValues = Encoding.UTF8.GetBytes(saltedTxt);
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashValues = sha256.ComputeHash(byteValues);
            string result = Convert.ToBase64String(hashValues);

            return result;
        }

        private string generateToken(long userId, string userName, string password)
        {
            // Universal variables.
            char[] char_tbl = new char[10];
            string final = null;

            // Token references.
            string date = DateTime.Today.ToString("yyMMdd");
            string userId_str = userId.ToString();

            // Phase 1 of generation.
            string phase1 = null;

            int loopIndex = 0;
            foreach (char c in userId_str)
            {
                if (loopIndex == 6) loopIndex = 0;
                phase1 += String.Format("{0}{1}", c, date[loopIndex++]);
            }

            // Phase 2.1 of generation.
            loopIndex = 0;
            for (int a = 0; a != 10; a++)
            {
                if (loopIndex == userName.Length) loopIndex = 0;
                char_tbl[a] = userName[loopIndex++];
            }

            // Phase 2.2 of generation.
            foreach (char c in phase1)
            {
                loopIndex = c - '0';
                final += char_tbl[loopIndex];
            }

            // Phase 3 of generation.
            string passPart = null;
            for(loopIndex = 0; loopIndex != 10; loopIndex++)
            {
                passPart += password[loopIndex];
            }

            string fin = String.Format("{0}{1}{0}", final, passPart);

            // Final encryption.
            string token = encrypt(final);

            return token;
        }

        private bool authenticateUser(long userId, string token)
        {
            DataTable userInfo = user.getUsers(userId);
            DataRow _dataRow = userInfo.Rows[0];
            string _userName = _dataRow["UserName"].ToString();
            string _password = _dataRow["Password"].ToString();
            string apiToken = generateToken(userId, _userName, _password);
            return token.Equals(apiToken);
        }

        private string clean(string text)
        {
            return text.ToLower().Trim();
        }
        #endregion

        #region "BETAS"

        [HttpGet]
        public string sampleProvider(long userId, string token)
        {
            string info = "I am a sensitive information.";

            bool userAuth = authenticateUser(userId, token);
            if (userAuth)
            {
                return info;
            }
            else
                return "Oops. it looks like you can't access this page!";
        }

        #endregion

        #region "USERS"

        #region "Login"
        [HttpGet]
        public DataTable Login(string userName, string password)
        {
            string errorMessage = null; // Error Message.

            // Validate parameters & fill errorMessage with violation.
            if (String.IsNullOrEmpty(userName)) errorMessage = "username_field_empty";
            else if (String.IsNullOrEmpty(password)) errorMessage = "password_field_empty";

            DataTable _dt = new DataTable();

            if (String.IsNullOrEmpty(errorMessage)) // Checks if errorMessage is null (means parameters are valid).
            {
                password = encrypt(password);
                _dt = user.Login(userName, password);

                DataRow dataRow = _dt.Rows[0];

                if (dataRow["return_message"].ToString().Equals("user_found"))
                {
                    long _userId = Convert.ToInt64(dataRow["UserID"].ToString());
                    string _userName = dataRow["UserName"].ToString();
                    string _password = dataRow["Password"].ToString();

                    _dt.Columns.Add("token", typeof(string));
                    dataRow["token"] = generateToken(_userId, _userName, _password);
                }
            }
            else
            {
                // If parameters are invalid, creates column 'return_message' with value of violation.
                _dt.Columns.Add("return_message", typeof(string));
                _dt.Rows.Add(errorMessage);
            }

            return _dt;
        }
        #endregion

        #region "SIGNUP"
        [HttpGet]
        public DataTable SignUp(int CreatedBy, string UserName, string Password, string RepeatPassword, string EmailAddress, string FirstName, string MiddleName, string LastName, int Role)
        {
            string errorMessage = null; // Error Message.
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex specialRegex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");

            // Validate parameters & fill errorMessage with violation.
            if (CreatedBy == 0) errorMessage = "security_breach";
            else if (!specialRegex.IsMatch(FirstName) || !specialRegex.IsMatch(MiddleName) || !specialRegex.IsMatch(LastName) || !specialRegex.IsMatch(UserName)) errorMessage = "contains_symbol";
            else if (String.IsNullOrEmpty(UserName)) errorMessage = "username_field_empty";
            else if (String.IsNullOrEmpty(Password)) errorMessage = "passord_field_empty";
            else if (String.IsNullOrEmpty(RepeatPassword)) errorMessage = "repass_field_empty";
            else if (!Password.Equals(RepeatPassword)) errorMessage = "password_mismatch";
            else if (String.IsNullOrEmpty(EmailAddress)) errorMessage = "email_field_empty";
            else if (!emailRegex.Match(EmailAddress).Success) errorMessage = "email_invalid_format";
            else if (String.IsNullOrEmpty(FirstName)) errorMessage = "fname_field_empty";
            else if (String.IsNullOrEmpty(MiddleName)) errorMessage = "mname_field_empty";
            else if (String.IsNullOrEmpty(LastName)) errorMessage = "lname_field_empty";

            DataTable _dt = new DataTable();
            if (String.IsNullOrEmpty(errorMessage)) // Checks if errorMessage is null (means parameters are valid).
            {
                user.UserName = clean(UserName);
                user.Password = encrypt(clean(Password));
                user.EmailAddress = clean(EmailAddress);
                user.FirstName = clean(FirstName);
                user.MiddleName = clean(MiddleName);
                user.LastName = clean(LastName);
                user.Role = Role;
                _dt = user.SignUp((int) CreatedBy, user);
            }
            else
            {
                // If parameters are invalid, creates column 'return_message' with value of violation.
                _dt.Columns.Add("return_message", typeof(string));
                _dt.Rows.Add(errorMessage);
            }

            return _dt;
        }
        #endregion


        [HttpGet]
        public DataTable getUsers(long userId)
        {
            return user.getUsers(userId);
        }
        [HttpGet]
        public DataTable deleteUsers(int UserID)
        {
            return user.deleteUser(UserID);
        }
        [HttpGet]
        public DataTable updateUsers(int UserID, string Password, string FirstName, string MiddleName, string LastName, byte Role)
        {
            user.UserID = UserID;
            user.Password = Password;
            user.FirstName = FirstName;
            user.MiddleName = MiddleName;
            user.LastName = LastName;
            user.Role = Role;
            return user.updateUsers(user);
        }
        #endregion

        #region "REGION"
        [HttpGet]
        public DataTable insertRegion(string regionName,string regionRemarks)
        {
            region.RegionName = regionName;
            region.RegionRemarks = regionRemarks;
            return region.insertRegion(region);
        }
        [HttpGet]
        public DataTable updateRegion(int regionID,string regionName,string regionRemarks)
        {
            region.RegionID = regionID;
            region.RegionName = regionName;
            region.RegionRemarks = regionRemarks;
            return region.updateRegion(region);
        }

        [HttpGet]
        public DataTable deleteRegion(int regionID)
        {
            return region.deleteRegion(regionID);
        }
        [HttpGet]
        public DataTable getRegion(int Status, int RegionID)
        {
            return region.getRegion(Status,RegionID);
        }
        #endregion
        #region "MASTER FILE"
        #region "AREA"

        [HttpGet]
        public DataTable getArea(int isActive)
        {
            return area.getArea(isActive);
        }

        [HttpGet]
        public DataTable insertArea(long createdBy, string areaName, string description, int regionCode, int isActive)
        {
            area.CreatedBy = createdBy;
            area.AreaName = areaName;
            area.Description = description;
            area.RegionCode = regionCode;
            area.IsActive = isActive;
            return area.insertArea(area);
        }

        [HttpGet]
        public DataTable updateArea(string areaName, string description, int isActive, int regionCode)
        {
            area.AreaName = areaName;
            area.Description = description;
            area.IsActive = isActive;
            area.RegionCode = regionCode;
            return area.updateArea(area);
        } // Error occurs during test

        [HttpGet]
        public DataTable deleteArea(int areaId, long createdBy)
        {
            return area.deleteArea(areaId, createdBy);
        } // Error occurs during test

        #endregion

        #region "BRANCH"

        [HttpGet]
        public DataTable getBranch(int Status,int IsActive)
        {
            return branch.getBranch(Status,IsActive);
        }
        [HttpGet]
        public DataTable insertBranch(string branchName, string branchRemarks, decimal latitude, decimal longtitude, int addressID,int areaID)
        {
            branch.BranchName = branchName;
            branch.BranchRemarks = branchName;
            branch.Latitude = Convert.ToDecimal(latitude);
            branch.Longtitude = Convert.ToDecimal(longtitude);
            branch.AddressID = addressID;
            branch.AreaID = areaID;
            return branch.insertBranch(branch);
        }
        [HttpGet]
        public DataTable updateBranch(int branchID,string branchName, string branchRemarks, decimal latitude, decimal longtitude, int addressID,int areaID)
        {
            branch.BranchID = branchID;
            branch.BranchName = branchName;
            branch.BranchRemarks = branchName;
            branch.Latitude = latitude;
            branch.Longtitude = longtitude;
            branch.AddressID = addressID;
            branch.AreaID = areaID;
            return branch.updateBranch(branch);
        } // Error occurs during test.

        [HttpGet]
        public DataTable deleteBranch(long branchId)
        {
            return branch.deleteBranch(branchId);
        } // Error occurs during test.

        #endregion
        #region "EMPLOYEE"
        [HttpGet]
        public DataTable insertEmployee(string firstName, string middleName, string lastName, string emailAddress, string contactNumber, int branchID)
        {
            emp.FirstName = firstName;
            emp.MiddleName = middleName;
            emp.LastName = lastName;
            emp.EmailAddress = emailAddress;
            emp.ContactNumber = contactNumber;
            emp.BranchID = branchID;
            return emp.insertEmployee(emp);
        }
        [HttpGet]
        public DataTable updateEmployee(int employeeID,string firstName, string middleName, string lastName, string emailAddress, string contactNumber, int branchID)
        {
            emp.EmployeeID = employeeID;
            emp.FirstName = firstName;
            emp.MiddleName = middleName;
            emp.LastName = lastName;
            emp.EmailAddress = emailAddress;
            emp.ContactNumber = contactNumber;
            emp.BranchID = branchID;
            return emp.updateEmployee(emp);
        }
        [HttpGet]
        public DataTable deleteEmployee(int employeeID)
        {
            return emp.deleteEmployee(employeeID);
        }
        [HttpGet]
        public DataTable getEmployee(int status, int employeeID)
        {
            return emp.getEmployee(status,employeeID);
        }
        #endregion
        #endregion
    }
}