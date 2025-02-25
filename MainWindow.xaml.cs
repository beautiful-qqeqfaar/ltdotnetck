using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using thick.Models;

namespace thick;


public partial class MainWindow : Window
{
    QlhocSinhContext db = new QlhocSinhContext();
    public MainWindow()
    {
        InitializeComponent();

        HienTHi();

        //cau combo box
        lop.ItemsSource = db.Lops.ToList(); //databse.list
        lop.DisplayMemberPath = "TenLop"; //ten gi day
        lop.SelectedValuePath = "MaLop";  //ma ma gi day
        lop.SelectedIndex = 0;
    }

    void HienTHi()
    {
        var q = db.HocSinhs.Select(h => new
        {
            MaHS = h.MaHs,
            HoTen = h.HoTen,
            NgaySinh = h.NgaySinh,
            GioiTinh = h.GioiTinh,
            TBLS = h.ConTbls,
            Lop = h.MaLopNavigation.TenLop,
            Tuoi = (DateTime.Now.Year - h.NgaySinh.Value.Year)
        });

        //q = from h in db.HocSinhs
        //    select new
        //    {
        //        MaHS = h.MaHs,
        //        HoTen = h.HoTen,
        //        NgaySinh = h.NgaySinh,
        //        GioiTinh = h.GioiTinh,
        //        TBLS = h.ConTbls,
        //        Lop = h.MaLopNavigation.TenLop,
        //        Tuoi = (DateTime.Now.Year - h.NgaySinh.Value.Year)
        //    };
        dt.ItemsSource = q.ToList();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (hoten.Text==""||mahs.Text=="")
        {
            MessageBox.Show("vui long nhap du thong tin");
            return;
        }
        var MaHS = mahs.Text;
        var HoTen = hoten.Text;
        var GioiTinh = radNam.IsChecked.Value? "Nam" : "Nu";
        var NgaySinh = ns.SelectedDate;
        var TBLS = tbls.IsChecked.Value ? "Co" : "Khong";
        string Lop = lop.SelectedValue.ToString();

        var Tuoi = (DateTime.Now.Year - NgaySinh.Value.Year);

        if (Tuoi < 10 || Tuoi > 15)
        {
            MessageBox.Show("tuoi tu 10-15");
            return;
        }

        //kiem tra id neu ko 
        if(db.HocSinhs.ToList().Find(h => h.MaHs == MaHS) != null)
        {
            MessageBox.Show("ma trung");
            return;
        }
        //var a = new
        //add(a) == add(new())
        //list.add
        db.HocSinhs.Add(new HocSinh()
        {
            MaHs = MaHS,
            HoTen = HoTen,
            NgaySinh = NgaySinh,
            GioiTinh = GioiTinh,
            ConTbls = TBLS,
            MaLop = Lop,
        });
        db.SaveChanges();

        //hien thi lai sau khi add
        HienTHi();

        //xoa
        //lay cai dang duoc chon 
        //xoa
    }

    private void W2(object sender, RoutedEventArgs e)
    {
        var w = new Window1();

        //ben window1 co cai dtata grid
        //m sql cho no ok chua

        //ten, lop, so hs nu
        //so la 2bang?
        // tinh tong cac hoc sinh gioi tinh nu
        //tong so hs nu la count (so luong0
        //neu de cho tinh tong tien thi phai dung sum(s=>s.tien);
        //trung
/*        var q = db.Lops.Select(l => new
        {
            MaHS = l.MaLop,
            Lop = l.TenLop,
            So = db.HocSinhs.Where(h => h.GioiTinh == "N?").Count(),
        });*/

        //de bai thay moi cai q
        //vi du lay item dang duoc chon?

        //vi du:
        //lay ma, ten, tong so hs nam? trong lop do?
        var q = db.Lops.Select(x => new
        {
            MaHS = x.MaLop,
            Lop =x.TenLop,
            So =db.HocSinhs
            .Where(h=> h.MaLop == x.MaLop && h.GioiTinh=="Nam" )

            .Count()
            
        });

        w.dt.ItemsSource = q.ToList();

        w.Show();//hien wind1
    }

    private void W3(object sender, RoutedEventArgs e)
    {
        // Lấy học sinh được chọn trong DataGrid
        var selectedItem = dt.SelectedItem;
        if (selectedItem == null)
        {
            MessageBox.Show("Vui lòng chọn học sinh cần xóa!");
            return;
        }
        // Lấy mã học sinh từ item đang được chọn
        string maHS = (string)selectedItem.GetType().GetProperty("MaHS")?.GetValue(selectedItem, null);

        // Tìm học sinh trong database
        var hocSinh = db.HocSinhs.FirstOrDefault(h => h.MaHs == maHS);
        if (hocSinh == null)
        {
            MessageBox.Show("Không tìm thấy học sinh trong cơ sở dữ liệu!");
            return;
        }
        // Xác nhận xóa
        var result = MessageBox.Show("Bạn có chắc chắn muốn xóa học sinh này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
            db.HocSinhs.Remove(hocSinh);
            db.SaveChanges();
            // Cập nhật lại danh sách
            HienTHi();
            MessageBox.Show("Xóa thành công!");
        }
    }
}