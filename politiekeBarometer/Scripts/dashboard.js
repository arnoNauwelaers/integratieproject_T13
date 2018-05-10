var items = "";
HideDelete();

var itemsHidden = document.getElementById("itemsHidden");

interact('.resize-drag')
    .draggable({
        // enable inertial throwing
        inertia: true,
        // keep the element within the area of it's parent
        restrict: {
            restriction: "parent",
            endOnly: true,
            elementRect: { top: 0, left: 0, bottom: 1, right: 1 }
        },
        // enable autoScroll
        autoScroll: true,

        // call this function on every dragmove event
        onmove: dragMoveListener,
        // call this function on every dragend event

    });

function dragMoveListener(event) {
    var target = event.target,
        // keep the dragged position in the data-x/data-y attributes
        x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx,
        y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

    // translate the element
    target.style.webkitTransform =
        target.style.transform =
        'translate(' + x + 'px, ' + y + 'px)';

    // update the posiion attributes
    target.setAttribute('data-x', x);
    target.setAttribute('data-y', y);
}

// this is used later in the resizing and gesture demos
window.dragMoveListener = dragMoveListener;


interact('.resize-drag')
    .draggable({
        onmove: window.dragMoveListener,
        restrict: {
            restriction: 'parent',
            elementRect: { top: 0, left: 0, bottom: 1, right: 1 }
        },
    })
    .resizable({
        // resize from all edges and corners
        edges: { left: true, right: true, bottom: true, top: true },

        // keep the edges inside the parent
        restrictEdges: {
            outer: 'parent',
            endOnly: true,
        },

        // minimum size
        restrictSize: {
            min: { width: 100, height: 50 },
        },

        inertia: true,
    })
    .on('resizemove', function (event) {
        var target = event.target,
            x = (parseFloat(target.getAttribute('data-x')) || 0),
            y = (parseFloat(target.getAttribute('data-y')) || 0);

        // update the element's style
        target.style.width = event.rect.width + 'px';
        target.style.height = event.rect.height + 'px';

        // translate when resizing from top or left edges
        x += event.deltaRect.left;
        y += event.deltaRect.top;

        target.style.webkitTransform = target.style.transform =
            'translate(' + x + 'px,' + y + 'px)';

        target.setAttribute('data-x', x);
        target.setAttribute('data-y', y);
    });
$(".grafiek").removeClass("resize-drag");
$("#editButton").click(function () {
    if ($("#editButton").hasClass("btn-success")) {
        ButtonToEdit();
    }
    else {
        ButtonToSave();
    }
});

function ButtonToEdit() {
    $(".grafiek").removeClass("resize-drag");
    $("#editButton").addClass("btn-primary");
    $("#editButton").removeClass("btn-success");
    $("#editButton").html("Edit");
    SaveCharts();
    HideDelete();
}

function ButtonToSave() {
    $(".grafiek").addClass("resize-drag");
    $("#editButton").removeClass("btn-primary");
    $("#editButton").addClass("btn-success");
    $("#editButton").html("Save");
    ShowDelete();
}

function HideDelete() {
    $(".deleteForm").hide();
}

function ShowDelete() {
    $(".deleteForm").show();
}

function SaveCharts() {
    var parentWidth = $(".resize-container").width();
    var text = '[';
    var teller = 0;
    $(".grafiek").each(function (index) {
        teller++;
        var id = $(this).attr("value");
        var X = $(this).attr("data-x");
        var Y = $(this).attr("data-y");
        if (isNaN(X)) {
            X = 0;
            Y = 0;
        }
        var Width = parentWidth / ($(this).css("width").replace("px", "") - 20);
        if (teller !== 1) {
            text += ',';
        }
        text += '{ "Id":"' + id + '" , "X":"' + X + '" , "Y":"' + Y + '" , "Width":"' + Width + '" }';
        $(".grafiek")
    })
    text += ']';
    $.ajax({
        dataType: "json",
        url: "/api/Basic/EditChart",
        method: "POST",
        data: { '': text }
    });
}

function addItem() {
    var ul = document.getElementById("itemList");
    var item = $("#items option:selected").val();
    var itemName = $("#items option:selected").text();
    var span = document.createElement("span");
    span.innerHTML = '<li>' + itemName + '</li>';
    ul.appendChild(span);
    if (items !== "") {
        items += " " + item;
    }
    else {
        items = item;
    }
    itemsHidden.value = items;
}

jQuery(document).ready(function () {
    $('.selectModal').select2({
        dropdownParent: $('#addModal'),
        width: '100%'
    });
});

$('#addModal').on('hidden.bs.modal', function () {
    items = "";
    itemsHidden.value = items;
    var ul = document.getElementById("itemList");
    ul.innerHTML = "";
});

function deleteChart(id) {
    var form = document.getElementById(id);
    form.submit();
}