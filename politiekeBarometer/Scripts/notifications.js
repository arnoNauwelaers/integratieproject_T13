function synchronizeDB() {
    $.getJSON("/api/Basic", function (data) {
        var items = [];
        $.each(data, function (key, val) {
            iziToast.info({ title: "Nieuwe melding!", message: val.Alert.Content, position: "topRight" });
        });
    });
    setTimeout(synchronizeDB, 5000);
}
//synchronizeDB();
//loadNotifications();
var ul = document.getElementById("notifications");
function loadNotifications() {
    $.getJSON("/api/Basic/GetNotifications", function (data) {
        ul.innerHTML = "";
        var items = [];
        $.each(data, function (key, val) {
            var span = document.createElement("span");
            span.innerHTML = '<div class="dropdown-divider"></div><a class="dropdown-item" href= "#" ><div class="dropdown-message small">' + val.Alert.Content + '</div></a >';
            ul.appendChild(span);
        });
    });
}