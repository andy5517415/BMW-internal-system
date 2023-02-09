var LoginInf = JSON.parse(sessionStorage.getItem("LoginInf", JSON.stringify(LoginInf)));
var EmpNumber = LoginInf.EmpNumber;
var DepID = LoginInf.DepID;
var Name = LoginInf.Name;
var PositionName = LoginInf.PositionName;
var Photo = LoginInf.Photo

switch (DepID) {
  case 1:
    Dep = "人事部";
    break;

  case 2:
    Dep = "生產部";
    break;
  case 3:
    Dep = "業務部";
    break;
  case 4:
    Dep = "採購部";
    break;

  default:
    break;
}


/*
var navInf = new Vue({
  el: '#navInf',
  data: {
    EmpNumber: EmpNumber,
    Name: Name,
    Dep: Dep
  }
})
*/

const navInf = {
  data() {
    return {
      navInf: {
        EmpNumber: EmpNumber,
        Name: Name,
        Dep: Dep,
        PositionName: PositionName,
        Photo: Photo
      }
    }
  },
  methods: {
    DoLogOut: function () {
      location.assign("Login_LogOut.html")
      sessionStorage.clear();
    }
  },
  mounted() {
    //監控
    var bef = 0;
    var aft = 0;
    var flag = true;
    var cnt = 0;

    function checkstatus() {
      console.log(cnt++);
      aft = 0;
      axios.get('/api/MonitoringProcessAreaStatus/').then(g => {
        this.abnormalcnt = g.data;
        //console.log(this.abnormalcnt);
        this.abnormalcnt.forEach(one => {
          (one.status == '異常') ? aft++ : '';
        });
        //console.log(`aft${aft}`);
        //console.log(`bef${bef}`);

        //定義第一次進入畫面不跳吐司訊息
        if (flag) {
          flag = !flag;
          bef = aft;
        }
        else if (aft < bef) {
          bef = aft;
        }
        else if (aft > bef) {
          alert('有新增異常狀態!');
          //$("#toastSucess").fadeIn("fast");
          //setTimeout(() => $("#toastSucess").fadeOut(), 2000);
          //setTimeout(() => window.location.reload(), 2000);
          bef = aft;
          //console.log('有異常');
        }

      })
      }


      if (DepID==2) {
        setInterval(checkstatus, 3000);
      }
    //end of 監控
  }
}

Vue.createApp(navInf).mount('#navInf')