﻿
	var xValues = @Html.Raw(Json.Encode(Kteam));
	console.log(xValues)
	var yValues = @Html.Raw(Json.Encode(Kteam1));




	var barColors = [
	"#b91d47",
	"#00aba9",
	"#2b5797",
	"#e8c3b9",
	"#1e7145"
	];

		new Chart("myChart", {
		type: "pie",
			data: {
		labels: xValues,
				datasets: [{
		backgroundColor: barColors,
					data: yValues
				}]
			},
			options: {
		title: {
		display: true,
					text: "Đồ thị phân loại sách"
				}
			}
		});
