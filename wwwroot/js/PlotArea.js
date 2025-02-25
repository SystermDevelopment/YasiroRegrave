const canvas = document.getElementById('plotCanvas'); // canvas要素を取得
const ctx = canvas.getContext('2d'); // 描画コンテキストを取得

// jsonファイル読込
let coordDatas;
var corrRate = 1.0;
var corrOffsX = 0.0;
var corrOffsY = 0.0;
document.addEventListener('DOMContentLoaded', () => {
    var areaFile = '/data/REIEN_' + ReienCode + '.json';
    fetch(areaFile)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response error:' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            // 座標データ読込成功
            coordDatas = data;

            areaDatas.forEach(function (area) {
                const areaCoords = coordDatas?.find(data => data["AreaCode"] == area.areaCode);

                var imagePath = '/images/REIEN_' + ReienCode + '.png';
                var img = new Image();
                img.src = imagePath;
                img.onload = function () {
                    //区画座標の補正計算
                    var cvs = document.getElementById("plotCanvas");
                    var cvsWidth = cvs.width;
                    var cvsHeight = cvs.height;
                    var imageWidth = img.width;
                    var imageHeight = img.height;
                    corrRate = Math.min(cvsWidth / imageWidth, cvsHeight / imageHeight);
                    corrOffsX = (cvsWidth - imageWidth * corrRate) / 2;
                    corrOffsY = (cvsHeight - imageHeight * corrRate) / 2;

                    areaCoords?.Coordinates?.forEach(function (coords) {
                        //区画座標の補正
                        coords.forEach(function (coord) {
                            coord.x = coord.x * corrRate + corrOffsX;
                            coord.y = coord.y * corrRate + corrOffsY;
                        });

                        // 矩形を描画
                        drawRect(coords);
                        //ctx.strokeStyle = 'black';
                        //ctx.strokeStyle = 'rgba(0, 0, 0, 0)';
                        //ctx.stroke();
                    });
                }

            });
        })
        .catch(error => {
            //console.error('There was a problem with the fetch operation:', error);
    });
});

// 矩形を描画
function drawRect(coordinates) {
    ctx.beginPath();
    ctx.moveTo(coordinates[0].x, coordinates[0].y);
    for (let i = 1; i < coordinates.length; i++) {
        ctx.lineTo(coordinates[i].x, coordinates[i].y);
    }

    ctx.closePath();
    //ctx.fillStyle = '#ffea07';
    ctx.fillStyle = 'rgba(255, 255, 255, 0)';
    ctx.strokeStyle = 'rgba(255, 255, 255, 0.3)';
    ctx.fill();
    ctx.stroke();
}


// 座標が矩形の内部にあるかを判定
function isInsidePolygon(x, y, coordinates) {
    let inside = false;
    let i, j = coordinates.length - 1;

    // (h)=(500) -> (h)=(400) *** #PlotAreaContainer(height)のheightと同値を定義 ***
    var baseHeight = 500;
    var mapContainer = document.getElementById('PlotAreaContainer');
    var mapHeight = mapContainer.offsetHeight;
    var corrRate = 1.0;
    if (mapHeight < baseHeight) {
        corrRate = mapHeight / baseHeight;
    }

    for (i = 0; i < coordinates.length; i++) {
        var ix = coordinates[i].x * corrRate;
        var iy = coordinates[i].y * corrRate;
        var jx = coordinates[j].x * corrRate;
        var jy = coordinates[j].y * corrRate;
        if ((iy > y) !== (jy > y) && (x < (jx - ix) * (y - iy) / (jy - iy) + ix)) {
            inside = !inside;
        }
        j = i;
    }
    return inside;
}

// マウスクリックイベント
canvas.addEventListener('click', function (event) {
    const clickX = event.clientX - canvas.getBoundingClientRect().left;
    const clickY = event.clientY - canvas.getBoundingClientRect().top;

    for (let i = 0; i < areaDatas.length; i++) {
        const area = areaDatas[i];
        const areaCoords = coordDatas?.find(data => data["AreaCode"] == area.areaCode);
        areaCoords?.Coordinates?.forEach(function (coords) {
            // 矩形選択
            const isInside = isInsidePolygon(clickX, clickY, coords);
            if (isInside) {
                window.location.href = "/PlotSelection?AreaCode=" + area.areaCode;
            }
        });
    }
});
