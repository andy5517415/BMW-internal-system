
var EmpNumber = JSON.parse(sessionStorage.getItem("EmpNumber", JSON.stringify(EmpNumber)));
var DepID = JSON.parse(sessionStorage.getItem("DepID", JSON.stringify(DepID)));
var Name = JSON.parse(sessionStorage.getItem("Name", JSON.stringify(Name)));

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