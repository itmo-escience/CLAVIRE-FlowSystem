using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Easis.Wfs.EasyFlow.Model;
using FlowChartEditor;
using Newtonsoft.Json.Linq;
using WFStarter.WFS;

namespace WFStarter
{
    public partial class WfStarter : Form
    {
        private int wfId;
        private bool monitoring = false;
        private string endpoint;
        private bool error = false;

        public WfStarter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JobRequest jr = new JobRequest();
            jr.Script = tbScript.Text;
            jr.ScriptDataContext = tbParams.Text;
            jr.FlowExecutionProperties = "";

            try
            {
                WFS.FlowSystemServiceClient cli = new FlowSystemServiceClient(endpoint);
                wfId = cli.PushJob(jr);
                Thread.Sleep(3000);
                monitoring = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (monitoring)
                {
                    WFS.FlowSystemServiceClient cli = new FlowSystemServiceClient(endpoint);
                    string str = cli.GetStatus(wfId);
                    tbOut.Text = str;

                    StreamWriter dotStream = new StreamWriter(new MemoryStream());

                    dotStream.WriteLine("digraph flow {");
                    dotStream.WriteLine("rankdir=\"BT\";");

                    //            string str =
                    //"{ \"id\": 1, \"state\": \"Active\", \"workflow\":{ \"id\": 1, \"nodes\":[ { \"id\":0,   \"type\":\"step\",   \"package\": \"SEMP\",   \"method\": \"zindo\",   \"parents\": [],   \"children\": [1,2],   \"state\": \"state_wait_results\" } ,{   \"id\":1,   \"type\":\"step\",   \"package\": \"SEMP\",   \"method\": \"zindo\",   \"parents\": [0],   \"children\": [2],   \"state\": \"state_init\" } ,{   \"id\":2,   \"type\":\"step\",   \"package\": \"SEMP\",   \"method\": \"zindo\",   \"parents\": [1,0],   \"children\": [],   \"state\": \"state_init\" } ] } } ";

                    JObject o = JObject.Parse(str);
                    int jid = (int)o["id"];
                    string jstate = (string)o["state"];

                    dotStream.WriteLine("label=\"Workflow monitor id: {0}, state: {1}\";", jid, jstate);

                    JContainer jwf = (JContainer)o["workflow"];
                    JArray jnodes = (JArray)jwf["nodes"];
                    foreach (var jnode in jnodes)
                    {
                        string node = String.Format("Step#{0} state: {1}", (int)jnode["id"], (string)jnode["state"]);

                        string color = "gray";
                        string comment = "";

                        switch ((string)jnode["state"])
                        {
                            case "state_run_start":
                                comment = "Preparing of job";
                                color = "yellow";
                                break;

                            case "state_wait_results":
                                comment = "Waiting for results";
                                color = "yellow";
                                break;

                            case "state_run_finish":
                                comment = "Handling results";
                                color = "yellow";
                                break;

                            case "state_post_section":
                            case "state_pre_section":
                                comment = "";
                                color = "yellow";
                                break;

                            case "state_init":
                                comment = "Not started";
                                color = "gray";
                                break;

                            case "state_started":
                                color = "yellow";
                                break;

                            case "state_finished":
                                color = "green";
                                comment = "Succeed";
                                break;
                            case "state_error":
                                color = "red";
                                comment = "Error in step";
                                break;
                        }

                        dotStream.WriteLine(
                            @"{0}[shape=""none"", margin=0, label=<
                    <TABLE border=""0"" cellborder=""1"" cellspacing=""0"">
                    <TR><TD BGCOLOR=""{2}"">{1}</TD></TR>;",
                            (int)jnode["id"], node, color);

                        dotStream.WriteLine("<TR><TD balign=\"LEFT\">");
                        dotStream.WriteLine("{0}.{1}", (string)jnode["package"], (string)jnode["method"]);
                        dotStream.WriteLine("<BR/><I>{0}</I>", comment);

                        dotStream.WriteLine("</TD></TR>");


                        dotStream.WriteLine("</TABLE>>];");

                        foreach (var parent in (JArray)jnode["parents"])
                        {
                            var srcName = (int)parent;
                            dotStream.WriteLine("{0} -> {1}", (int)jnode["id"], (int)parent);
                        }
                    }

                    dotStream.WriteLine("}");
                    dotStream.Flush();

                    dotStream.BaseStream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(dotStream.BaseStream);
                    string dotCode = reader.ReadToEnd();

                    FlowChart chart = new FlowChart();
                    chart.SetDotCode(dotCode);
                    pbOut.Image = chart.ChartImage;


                }
                else
                {
                    try
                    {
                        string code = tbScript.Text;
                        FlowChart chart = new FlowChart();
                        bool result = chart.Parse(code);
                        pbOut.Image = chart.ChartImage;

                        error = false;
                        tbScript.BackColor = Color.White;
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        tbScript.BackColor = Color.PaleVioletRed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void WfStarter_Load(object sender, EventArgs e)
        {
            timer1_Tick(this,new EventArgs());

            ClientSection clientSection = 
                ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

            ChannelEndpointElementCollection endpointCollection =
                clientSection.ElementInformation.Properties[string.Empty].Value as ChannelEndpointElementCollection;

            //var eps = from ChannelEndpointElement xx in endpointCollection
            //          select new { Name = xx.Name, Address = xx.Address };

            foreach (ChannelEndpointElement endpointElement in endpointCollection)
            {
                comboBox1.Items.Add(endpointElement.Name);
            }
            comboBox1.SelectedIndex = 0;
            endpoint = (string)comboBox1.SelectedItem;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            endpoint = (string)comboBox1.SelectedItem;
        }
    }
}
