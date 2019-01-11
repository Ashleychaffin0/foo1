using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Install-Package Microsoft.EntityFrameworkCore -Version 2.1.0

namespace TryEntityFrameworkCore20 {
	public partial class TryEntityFrameworkCore20 : Form {
		public TryEntityFrameworkCore20() {
			InitializeComponent();
		}
	}
}

namespace ComputerInventory.Models {
	public partial class OperatingSys {
		public OperatingSys() {
			Machine = new HashSet<Machine>();
		}
		public int OperatingSysId { get; set; }
		public string Name { get; set; }
		public bool StillSupported { get; set; }
		public ICollection<Machine> Machine { get; set; }
	}
}

namespace ComputerInventory.Models {
	public partial class MachineType {
		public MachineType() {
			Machine = new HashSet<Machine>();
		}
		public int MachineTypeId { get; set; }
		public string Description { get; set; }
		public ICollection<Machine> Machine { get; set; }
	}
}

namespace ComputerInventory.Models {
	public partial class WarrantyProvider {
		public WarrantyProvider() {
			MachineWarranty = new HashSet<MachineWarranty>();
		}
		public int WarrantyProviderId { get; set; }
		public string ProviderName { get; set; }
		public int? SupportExtension { get; set; }
		public string SupportNumber { get; set; }
		public ICollection<MachineWarranty> MachineWarranty { get; set; }
	}
}

namespace ComputerInventory.Models {
	public partial class MachineWarranty {
		public int MachineWarrantyId { get; set; }
		public string ServiceTag { get; set; }
		public DateTime WarrantyExpiration { get; set; }
		public int MachineId { get; set; }
		public int WarrantyProviderId { get; set; }
		public WarrantyProvider WarrantyProvider { get; set; }
	}
}

namespace ComputerInventory.Models {
	public partial class SupportTicket {
		public SupportTicket() {
			SupportLog = new HashSet<SupportLog>();
		}
		public int SupportTicketId { get; set; }
		public DateTime DateReported { get; set; }
		public DateTime? DateResolved { get; set; }
		public string IssueDescription { get; set; }
		public string IssueDetail { get; set; }
		public string TicketOpenedBy { get; set; }
		public int MachineId { get; set; }
		public Machine Machine { get; set; }
		public ICollection<SupportLog> SupportLog { get; set; }
	}
}

namespace ComputerInventory.Models {
	public partial class SupportLog {
		public int SupportLogId { get; set; }
		public DateTime SupportLogEntryDate { get; set; }
		public string SupportLogEntry { get; set; }
		public string SupportLogUpdatedBy { get; set; }
		public int SupportTicketId { get; set; }
		public SupportTicket SupportTicket { get; set; }
	}
}

namespace ComputerInventory.Models {
	public partial class Machine {
		public Machine() {
			SupportTicket = new HashSet<SupportTicket>();
		}
		public int MachineId { get; set; }
		public string Name { get; set; }
		public string GeneralRole { get; set; }
		public string InstalledRoles { get; set; }
		public int OperatingSysId { get; set; }
		public int MachineTypeId { get; set; }
		public MachineType MachineType { get; set; }
		public OperatingSys OperatingSys { get; set; }
		public ICollection<SupportTicket> SupportTicket { get; set; }
	}
}
