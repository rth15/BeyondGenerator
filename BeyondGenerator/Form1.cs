using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace BeyondGenerator
{
    public partial class Form1 : Form
    {
        public BeyondConfig config { get; set; }

        public Form1()
        {
            InitializeComponent();
            this.config = new BeyondConfig();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string path = "C:\\temp\\config.json";
                JObject o1 = JObject.Parse(File.ReadAllText(@"c:\temp\config.json"));
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    this.config = JsonConvert.DeserializeObject<BeyondConfig>(json);
                }
                txtConfigName.Text = "config";
            }
            catch (Exception f)
            {
                MessageBox.Show(string.Format("Place config file at C:\\temp\\config.json for autoload. Click Ok if creating a new template.\n{0}", f.Message));
            }
            finally
            {
                RefreshTreeView();
            }
        }

        private void RefreshTreeView(string json = "")
        {
            if (json.Trim().Length == 0)
            {
                json = JsonConvert.SerializeObject(config);
            }
            try
            {
                JObject obj = JObject.Parse(json);
                treeView1.Nodes.Clear();
                TreeNode parent = Json2Tree(obj);
                parent.Text = txtConfigName+".json";
                treeView1.Nodes.Add(parent);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

            if (config.General.TimedPointsReward.Enabled == true)
            {
                checkTPREnable.Checked = true;
            }

            if (checkTPREnable.Checked)
                lblTPR.ForeColor = Color.Green;
            else if (!checkTPREnable.Checked)
                lblTPR.ForeColor = Color.Red;


            numTPRAmt.Value = config.General.TimedPointsReward.Amount;
            numTPRInterval.Value = config.General.TimedPointsReward.Interval;
            numShopIPP.Value = config.General.ItemsPerPage;
            numShopDisplayTime.Value = config.General.ShopDisplayTime;

        }


        private void Save_Click(object sender, EventArgs e)
        {
            string path = string.Format("C:\\temp\\{0}.json", txtConfigName.Text);

            string json = JsonConvert.SerializeObject(config);
            using (StreamWriter file = File.CreateText(path))
            {
                file.Write(json);
            }
        }

        private void btnAddKit_Click(object sender, EventArgs e)
        {
            Kit newKit = new Kit();

            string Name = txtKitName.Text;
            newKit.Price = (int)numKitPrice.Value;
            newKit.DefaultAmount = (int)numKitNo.Value;
            newKit.KitItems = new List<KitItem>();
            newKit.KitDinos = new List<KitDino>();


            foreach (DataGridViewRow row in dgvKitItems.Rows)
            {
                if (!(row.Cells["KitItemBp"].FormattedValue.ToString().Trim() == ""))
                {
                    KitItem kitItem = new KitItem();

                    kitItem.amount = int.Parse(row.Cells["KitItemAmount"].FormattedValue.ToString());
                    kitItem.quality = int.Parse(row.Cells["KitItemQuality"].FormattedValue.ToString());
                    kitItem.blueprint = row.Cells["KitItemBp"].FormattedValue.ToString();
                    kitItem.forceBlueprint = true;

                    newKit.KitItems.Add(kitItem);
                }
            }


            if (dgvKitDinos.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in dgvKitDinos.Rows)
                {
                    if (!(row.Cells["KitDinoBp"].FormattedValue.ToString().Trim() == ""))
                    {
                        KitDino kitDino = new KitDino();

                        kitDino.Level = int.Parse(row.Cells["KitDinoLevel"].FormattedValue.ToString());
                        kitDino.className = row.Cells["KitDinoBp"].FormattedValue.ToString();

                        newKit.KitDinos.Add(kitDino);
                    }
                }
            }
            config.Kits.Add(Name, newKit);

            RefreshTreeView();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            Items newItems = new Items();

            string Name = txtShopName.Text;
            newItems.Description = txtShopDescription.Text;
            newItems.Price = (int)numShopPrice.Value;
            newItems.ItemList = new List<Item>();

            foreach (DataGridViewRow row in dgvShopItems.Rows)
            {
                if (!(row.Cells["ShopItemBp"].FormattedValue.ToString().Trim() == ""))
                {
                    Item item = new Item();
                    item.Type = "item";
                    item.Amount = int.Parse(row.Cells["ShopItemAmount"].FormattedValue.ToString());
                    item.Quality = int.Parse(row.Cells["ShopItemQuality"].FormattedValue.ToString());
                    item.BluePrint = row.Cells["ShopItemBp"].FormattedValue.ToString();
                    item.ForceBlueprint = true;

                    newItems.ItemList.Add(item);
                }
            }

            config.ShopItems.Items.Add(Name, newItems);


            RefreshTreeView();
        }

        private void btnAddDino_Click(object sender, EventArgs e)
        {
            Dino newDino = new Dino();

            string Name = txtShopName.Text;
            newDino.Description = txtShopDescription.Text;
            newDino.Type = "dino";
            newDino.Price = (int)numShopPrice.Value;
            newDino.className = txtShopDinoClassName.Text;
            newDino.Level = (int)numShopDinoLevel.Value;

            config.ShopItems.Dinos.Add(Name, newDino);

            RefreshTreeView();
        }

        private void checkTPREnable_Click(object sender, EventArgs e)
        {
            config.General.TimedPointsReward.Enabled = checkTPREnable.Enabled;

            RefreshTreeView();
        }

        private void numTPRInterval_ValueChanged(object sender, EventArgs e)
        {
            config.General.TimedPointsReward.Interval = (int)numTPRInterval.Value;

            RefreshTreeView();
        }

        private void numTPRAmt_ValueChanged(object sender, EventArgs e)
        {
            config.General.TimedPointsReward.Amount = (int)numTPRAmt.Value;

            RefreshTreeView();
        }

        private void numShopIPP_ValueChanged(object sender, EventArgs e)
        {
            config.General.ItemsPerPage = (int)numShopIPP.Value;

            RefreshTreeView();
        }

        private void numShopDisplayTime_ValueChanged(object sender, EventArgs e)
        {
            config.General.ShopDisplayTime = (int)numShopDisplayTime.Value;

            RefreshTreeView();
        }

        private TreeNode Json2Tree(JObject obj, bool inner = false, string parentName = "")
        {
            //create the parent node
            TreeNode parent = new TreeNode();
            //loop through the obj. all token should be pair<key, value>
            foreach (var token in obj)
            {


                //create the child node
                TreeNode child = new TreeNode();
                child.Text = token.Key.ToString();
                //check if the value is of type obj recall the method
                if (token.Value.Type.ToString() == "Object")
                {
                    // child.Text = token.Key.ToString();
                    //create a new JObject using the the Token.value
                    JObject o = (JObject)token.Value;
                    //recall the method
                    child = Json2Tree(o, true, token.Key.ToString());
                    //add the child to the parentNode
                    parent.Nodes.Add(child);
                }
                //if type is of array
                else if (token.Value.Type.ToString() == "Array")
                {
                    int ix = -1;
                    //  child.Text = token.Key.ToString();
                    //loop though the array
                    foreach (var itm in token.Value)
                    {
                        //check if value is an Array of objects
                        if (itm.Type.ToString() == "Object")
                        {
                            TreeNode objTN = new TreeNode();
                            //child.Text = token.Key.ToString();
                            //call back the method
                            ix++;

                            JObject o = (JObject)itm;
                            objTN = Json2Tree(o);
                            objTN.Text = token.Key.ToString() + "[" + ix + "]";
                            child.Nodes.Add(objTN);
                            //parent.Nodes.Add(child);
                        }
                        //regular array string, int, etc
                        else if (itm.Type.ToString() == "Array")
                        {
                            ix++;
                            TreeNode dataArray = new TreeNode();
                            foreach (var data in itm)
                            {
                                dataArray.Text = token.Key.ToString() + "[" + ix + "]";
                                dataArray.Nodes.Add(data.ToString());
                            }
                            child.Nodes.Add(dataArray);
                        }

                        else
                        {
                            child.Nodes.Add(itm.ToString());
                        }
                    }
                    parent.Nodes.Add(child);
                }
                //if type is of array
                else if (token.Value.Type.ToString() == "Dictionary")
                {
                    int ix = -1;
                    //  child.Text = token.Key.ToString();
                    //loop though the array
                    foreach (var itm in token.Value)
                    {
                        //check if value is an Array of objects
                        if (itm.Type.ToString() == "Object")
                        {
                            TreeNode objTN = new TreeNode();
                            //child.Text = token.Key.ToString();
                            //call back the method
                            ix++;

                            JObject o = (JObject)itm;
                            objTN = Json2Tree(o);
                            objTN.Text = token.Key.ToString() + "[" + ix + "]";
                            child.Nodes.Add(objTN);
                            //parent.Nodes.Add(child);
                        }
                        //regular array string, int, etc
                        else if (itm.Type.ToString() == "Array")
                        {
                            ix++;
                            TreeNode dataArray = new TreeNode();
                            foreach (var data in itm)
                            {
                                dataArray.Text = token.Key.ToString() + "[" + ix + "]";
                                dataArray.Nodes.Add(data.ToString());
                            }
                            child.Nodes.Add(dataArray);
                        }

                        else
                        {
                            child.Nodes.Add(itm.ToString());
                        }
                    }
                    parent.Nodes.Add(child);
                }
                else
                {
                    //if token.Value is not nested
                    // child.Text = token.Key.ToString();
                    //change the value into N/A if value == null or an empty string 
                    if (token.Value.ToString() == "")
                        child.Nodes.Add("N/A");
                    else
                        child.Nodes.Add(token.Value.ToString());
                    parent.Nodes.Add(child);
                }
                //change the display Content of the parent
                if (!inner)
                    parent.Text = token.Key.ToString();
                else
                    parent.Text = parentName;
                //else 
                //    parent.Text
            }
            return parent;

        }
    }
}