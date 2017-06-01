using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using DataModel;

namespace TCPClient
{
    public partial class Form1 : Form
    {
        DataModel.Data data { get; set; }
        //public WCFServiceLib.WCFServiceClient client { get; set; }

        public Form1()
        {
            InitializeComponent();
            TCPRPCClient.Connect("127.0.0.1", 17777);
            //client = new WCFServiceLib.WCFServiceClient("BasicHttpBinding_IWCFService");
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = false;
        }


        private void btnAddUser_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlAddUserData.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlAddUserData) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnAddAddress_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlAddAddressData.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlAddAddressData) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlAddOrderData.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlAddOrderData) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }


        private void btnRemoveUser_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlRemoveUser.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlRemoveUser) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnRemoveAddress_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlDelAddress.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlDelAddress) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnRemoveOrder_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlDelOrder.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlDelOrder) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }


        private void GetData()
        {
            string jsondata = TCPRPCClient.GetData();
            data = JsonConvert.DeserializeObject<Data>(jsondata, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            rtbUsers.Clear();
            rtbAddresses.Clear();
            rtbOrders.Clear();

            foreach (var user in data.UserList)
            {
                rtbUsers.Text += "Id: " + user.Id + " | Name: " + user.Name + "\n";
                rtbUsers.Text += "Addresses: ";
                foreach (var addr in user.AddressList)
                {
                    rtbUsers.Text += addr + "; ";
                }
                rtbUsers.Text += "\nOrders: ";
                foreach (var ord in user.OrderList)
                {
                    rtbUsers.Text += ord + "; ";
                }
                rtbUsers.Text += "\n\n";
            }

            foreach (var addr in data.AddressList)
            {
                rtbAddresses.Text += "Id: " + addr.Id + " | City: " + addr.City + " | Street: " + addr.Street
                    + " | Build: " + addr.Build + " | Flat: " + addr.Flat + "\n";
                rtbAddresses.Text += "Users: ";
                foreach (var user in addr.UserList)
                {
                    rtbAddresses.Text += user + "; ";
                }
                rtbAddresses.Text += "\nOrders: ";
                foreach (var ord in addr.OrderList)
                {
                    rtbAddresses.Text += ord + "; ";
                }
                rtbAddresses.Text += "\n\n";
            }

            foreach (var ord in data.OrderList)
            {
                rtbOrders.Text += "Id: " + ord.Id + " | GoodName: " + ord.GoodName
                    + " | UserId: " + ord.UserID + " | AddressId: " + ord.AddressID;
                rtbOrders.Text += "\n\n";
            }
        }


        private void AddUser()
        {
            if (tboxUserName.Text != "")
            {
                pnlEnterData.Visible = false;
                //pnlAddUserData.Visible = false;

                MessageBox.Show("User added. Returned Id: " + TCPRPCClient.AddUser(tboxUserName.Text));
                GetData();
            }
            else lblMessage.Text = "Some fields are empty!";
        }

        private void AddAddress()
        {
            if (tboxBuild.Text != "" && tboxCity.Text != "" && tboxFlat.Text != "" && tboxStreet.Text != "")
            {
                pnlEnterData.Visible = false;
                //pnlAddAddressData.Visible = false;

                MessageBox.Show("Address added. Returned Id: " + TCPRPCClient.AddAddress(tboxCity.Text, tboxStreet.Text,
                    Convert.ToInt32(tboxBuild.Text), Convert.ToInt32(tboxFlat.Text)));
                GetData();
            }
            else lblMessage.Text = "Some fields are empty!";
        }

        private void AddOrder()
        {

            if (tboxGoodName.Text != "")
            {
                pnlEnterData.Visible = false;
                //pnlAddAddressData.Visible = false;

                MessageBox.Show("Address added. Returned Id: " + TCPRPCClient.AddOrder(tboxGoodName.Text));
                GetData();
            }
            else lblMessage.Text = "Some fields are empty!";
        }


        private void RemoveUser()
        {
            try
            {
                if (tboxDelUserId.Text != "")
                {
                    int UserId = Convert.ToInt32(tboxDelUserId.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.RemoveUser(UserId))
                    {
                        MessageBox.Show("User with id " + UserId + " deleted succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("User with id " + UserId + " doesn't exist.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RemoveAddress()
        {
            try
            {
                if (tboxDelAddress.Text != "")
                {
                    int AddrId = Convert.ToInt32(tboxDelAddress.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.RemoveAddress(AddrId))
                    {
                        MessageBox.Show("User with id " + AddrId + " deleted succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("User with id " + AddrId + " doesn't exist.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RemoveOrder()
        {
            try
            {
                if (tboxDelOrder.Text != "")
                {
                    int OrdId = Convert.ToInt32(tboxDelOrder.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.RemoveOrder(OrdId))
                    {
                        MessageBox.Show("User with id " + OrdId + " deleted succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("User with id " + OrdId + " doesn't exist.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void AddLinkUserToAddress()
        {
            try
            {
                if (tboxUALinkAddress.Text != "" && tboxUALinkUser.Text != "")
                {
                    int UserId = Convert.ToInt32(tboxUALinkUser.Text);
                    int AddrId = Convert.ToInt32(tboxUALinkAddress.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.AddUserToAddress(UserId, AddrId))
                    {
                        MessageBox.Show("Link UserId: " + UserId + " | AddressId: " + AddrId + " created succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Link creation failed.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception)
            {
                MessageBox.Show("No changes were done.");
            }
        }

        private void AddLinkAddressToUser()
        {
            try
            {
                if (tboxAddAULinkAddress.Text != "" && tboxAddAULinkUser.Text != "")
                {
                    int UserId = Convert.ToInt32(tboxAddAULinkUser.Text);
                    int AddrId = Convert.ToInt32(tboxAddAULinkAddress.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.AddAddressToUser(AddrId, UserId))
                    {
                        MessageBox.Show("Link AddrId: " + AddrId + " | UserId: " + UserId + " created succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Link creation failed.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception)
            {
                MessageBox.Show("No changes were done.");
            }
        }

        private void AddLinkOrderToUser()
        {
            try
            {
                if (tboxAddOULinkOrder.Text != "" && tboxAddOULinkUser.Text != "")
                {
                    int UserId = Convert.ToInt32(tboxAddOULinkUser.Text);
                    int OrdId = Convert.ToInt32(tboxAddOULinkOrder.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.AddOrderToUser(OrdId, UserId))
                    {
                        MessageBox.Show("Link OrderId: " + OrdId + " | UserId: " + UserId + " created succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Link creation failed.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception)
            {
                MessageBox.Show("No changes were done.");
            }
        }

        private void AddLinkOrderToAddress()
        {
            try
            {
                if (tboxAddOALinkOrder.Text != "" && tboxAddOALinkAddress.Text != "")
                {
                    int AddrId = Convert.ToInt32(tboxAddOALinkAddress.Text);
                    int OrdId = Convert.ToInt32(tboxAddOALinkOrder.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.AddOrderToAddress(OrdId, AddrId))
                    {
                        MessageBox.Show("Link OrderId: " + OrdId + " | AddressId: " + AddrId + " created succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Link creation failed.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception)
            {
                MessageBox.Show("No changes were done.");
            }
        }


        private void RemoveLinkUserToAddress()
        {
            try
            {
                if (tboxDelUALinkAddress.Text != "" && tboxDelUALinkUser.Text != "")
                {
                    int UserId = Convert.ToInt32(tboxDelUALinkUser.Text);
                    int AddrId = Convert.ToInt32(tboxDelUALinkAddress.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.RemoveUserFromAddress(UserId, AddrId))
                    {
                        MessageBox.Show("Link UserId: " + UserId + " | AddressId: " + AddrId + " deleted succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Link deletion failed.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception)
            {
                MessageBox.Show("No changes were done.");
            }
        }

        private void RemoveLinkAddressToUser()
        {
            try
            {
                if (tboxDelAULinkAddress.Text != "" && tboxDelAULinkUser.Text != "")
                {
                    int UserId = Convert.ToInt32(tboxDelAULinkUser.Text);
                    int AddrId = Convert.ToInt32(tboxDelAULinkAddress.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.RemoveAddressFromUser(AddrId, UserId))
                    {
                        MessageBox.Show("Link AddressId: " + AddrId + " | UserId: " + UserId + " deleted succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Link deletion failed.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception)
            {
                MessageBox.Show("No changes were done.");
            }
        }

        private void RemoveLinkOrderToUser()
        {
            try
            {
                if (tboxDelOULinkOrder.Text != "" && tboxDelOULinkUser.Text != "")
                {
                    int OrdId = Convert.ToInt32(tboxDelOULinkOrder.Text);
                    int UserId = Convert.ToInt32(tboxDelOULinkUser.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.RemoveOrderFromUser(OrdId, UserId))
                    {
                        MessageBox.Show("Link OrderId: " + OrdId + " | UserId: " + UserId + " deleted succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Link deletion failed.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception)
            {
                MessageBox.Show("No changes were done.");
            }
        }

        private void RemoveLinkOrderToAddress()
        {
            try
            {
                if (tboxDelOALinkOrder.Text != "" && tboxDelOALinkAddress.Text != "")
                {
                    int OrdId = Convert.ToInt32(tboxDelOALinkOrder.Text);
                    int AddrId = Convert.ToInt32(tboxDelOALinkAddress.Text);
                    pnlEnterData.Visible = false;
                    if (TCPRPCClient.RemoveOrderFromAddress(OrdId, AddrId))
                    {
                        MessageBox.Show("Link OrderId: " + OrdId + " | AddressId: " + AddrId + " deleted succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Link deletion failed.");
                    }
                    GetData();
                }
                else lblMessage.Text = "Some fields are empty!";
            }
            catch (Exception)
            {
                MessageBox.Show("No changes were done.");
            }
        }


        private void btnOkAddUser_Click(object sender, EventArgs e)
        {
            AddUser();
        }

        private void btnOkAddAddress_Click(object sender, EventArgs e)
        {
            AddAddress();
        }

        private void btnOkAddOrder_Click(object sender, EventArgs e)
        {
            AddOrder();
        }


        private void btnRemUser_Click(object sender, EventArgs e)
        {
            RemoveUser();
        }

        private void btnDelAddress_Click(object sender, EventArgs e)
        {
            RemoveAddress();
        }

        private void btnDelOrder_Click(object sender, EventArgs e)
        {
            RemoveOrder();
        }


        private void btnAddLinkUserAddress_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlAddUALink.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlAddUALink) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnLinkOrderUser_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlAddOULink.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlAddOULink) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnAddLinkAddressUser_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlAddAULink.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlAddAULink) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnLinkOrderAddress_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlAddOALink.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlAddOALink) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }


        private void btnDelUserAddress_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlDelUALink.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlDelUALink) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnDelOrderUser_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlDelOULink.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlDelOULink) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnDelAddressUser_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlDelAULink.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlDelAULink) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }

        private void btnDelOrderAddress_Click(object sender, EventArgs e)
        {
            pnlEnterData.Visible = true;
            pnlDelOALink.Visible = true;
            foreach (var panel in pnlEnterData.Controls)
            {
                if (panel is Panel) if ((panel as Panel) != pnlDelOALink) (panel as Panel).Visible = false;
            }
            lblMessage.Text = "Fill all empty fields";
        }


        private void btnAddUALink_Click(object sender, EventArgs e)
        {
            AddLinkUserToAddress();
        }

        private void btnAddAULink_Click(object sender, EventArgs e)
        {
            AddLinkAddressToUser();
        }

        private void btnAddOULink_Click(object sender, EventArgs e)
        {
            AddLinkOrderToUser();
        }

        private void btnAddOALink_Click(object sender, EventArgs e)
        {
            AddLinkOrderToAddress();
        }


        private void btnDelUALink_Click(object sender, EventArgs e)
        {
            RemoveLinkUserToAddress();
        }

        private void btnDelAULink_Click(object sender, EventArgs e)
        {
            RemoveLinkAddressToUser();
        }

        private void btnDelOULink_Click(object sender, EventArgs e)
        {
            RemoveLinkOrderToUser();
        }

        private void btnDelOALink_Click(object sender, EventArgs e)
        {
            RemoveLinkOrderToAddress();
        }


        private void tboxOnlyNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
