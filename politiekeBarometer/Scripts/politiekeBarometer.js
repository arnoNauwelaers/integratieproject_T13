function CreateChart(canvasId, typeChart, labelsChart, dataSets) {
    var canvasId = new Chart(canvasId, {
        type: typeChart,
        data: {
            labels: labelsChart,
            datasets: dataSets
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}