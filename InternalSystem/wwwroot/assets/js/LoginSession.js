var LoginInf = JSON.parse(sessionStorage.getItem("LoginInf", JSON.stringify(LoginInf)));
var EmpNumber = LoginInf.EmpNumber;
var DepID = LoginInf.DepID;
var Name = LoginInf.Name;

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
  case 1:
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
        Dep: Dep
      }
    }
  }
}

Vue.createApp(navInf).mount('#navInf')