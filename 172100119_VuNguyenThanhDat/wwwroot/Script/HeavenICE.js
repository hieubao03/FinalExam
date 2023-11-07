function show() {
    var r = document.getElementsByClassName("nav")[0];
    if (r.style.display !== "block") r.style.display = "block";
    else r.style.display = "none";
}

function on_off(btn) {
    var bt = btn.parentElement;
    var on = bt.getElementsByClassName("on")[0];
    var off = bt.getElementsByClassName("off")[0];
    if (on.style.display == "inline") {
        on.style.display = "none";
        off.style.display = "inline";
        btn.innerHTML = "Thêm";
    } else {
        on.style.display = "inline";
        off.style.display = "none";
        btn.innerHTML = "Đóng";
    }
}
