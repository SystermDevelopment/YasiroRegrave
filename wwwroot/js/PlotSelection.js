const canvas = document.getElementById('plotCanvas'); // canvas要素を取得
const ctx = canvas.getContext('2d'); // 描画コンテキストを取得

// jsonファイル読込
let coordDatas;
var corrRate = 1.0;
var corrOffsX = 0.0;
var corrOffsY = 0.0;
document.addEventListener('DOMContentLoaded', () => {
    var areaFile = '/data/AREA_' + ReienCode + '_' + AreaCode + '.json';
    fetch(areaFile)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response error:' + response.statusText);
            }
            return response.json();
            //return response.arrayBuffer();
        })
        .then(data => {
            // 座標データ読込成功
            coordDatas = data;

            let isCanvasInitialized = false;

            sectionDatas.forEach(function (section) {
                const sectionCoords = coordDatas?.find(data => data["SectionCode"] == section.sectionCode);
                // 空きあり場合（空き以外も初回のみ描画）
                if (sectionCoords && section.noReserveCount >= 0) {
                    // ↓↓↓ 画像作成用処理（通常コメント化）↓↓↓
                    //section.noReserveCount = 1;
                    //if (section.sectionCode == "緑風") {
                    //    section.noReserveCount = 1;
                    //}
                    // ↑↑↑ 画像作成用処理（通常コメント化）↑↑↑

                    //const sectionCoords = coordDatas.find(data => data["SectionCode"] == section.sectionCode);

                    var imagePath = '/images/AREA_' + ReienCode + '_' + AreaCode + '.png';
                    var img = new Image();
                    img.src = imagePath;
                    img.onload = function () {
                        //区画座標の補正計算
                        var cvs = document.getElementById("plotCanvas");

                        // キャンバスサイズを1回だけ更新
                        if (!isCanvasInitialized) {
                            cvs.width = cvs.clientWidth;
                            cvs.height = cvs.clientHeight;
                            ctx.clearRect(0, 0, cvs.width, cvs.height);
                            isCanvasInitialized = true;
                        }

                        var cvsWidth = cvs.width;
                        var cvsHeight = cvs.height;
                        var imageWidth = img.width;
                        var imageHeight = img.height;
                        corrRate = Math.min(cvsWidth / imageWidth, cvsHeight / imageHeight);
                        corrOffsX = (cvsWidth - imageWidth * corrRate) / 2;
                        corrOffsY = (cvsHeight - imageHeight * corrRate) / 2;

                        sectionCoords.Coordinates.forEach(function (coords) {
                            //区画座標の補正
                            coords.forEach(function (coord) {
                                coord.x = coord.x * corrRate + corrOffsX;
                                coord.y = coord.y * corrRate + corrOffsY;
                            });

                            // 矩形と名前を描画
                            drawRect(coords, section.noReserveCount);
                            //drawName(coords, section.sectionName);
                            ctx.strokeStyle = 'black';
                            ctx.stroke();
                        });
                        // 名前のみ描画
                        sectionCoords.Coordinates.forEach(function (coords) {
                            setTimeout(function () {
                                drawName(coords, section.sectionName);
                            }, 0);
                        });
                    }
                }
            });
        })
        .catch(error => {
            //console.error('There was a problem with the fetch operation:', error);
    });
});

// 矩形を描画
function drawRect(coordinates, reserve) {
    ctx.beginPath();
    ctx.moveTo(coordinates[0].x, coordinates[0].y);
    for (let i = 1; i < coordinates.length; i++) {
        ctx.lineTo(coordinates[i].x, coordinates[i].y);
    }

    ctx.closePath();
    ctx.fillStyle = reserve > 0 ? '#ffea07' : 'white';
    ctx.fill();
    ctx.stroke();
}

// 名前を描画
function drawName(coordinates, name) {
    const center = calculateCenter(coordinates);
    const maxWidth = coordinates.reduce((max, point) => Math.max(max, point.x), coordinates[0].x) - coordinates.reduce((min, point) => Math.min(min, point.x), coordinates[0].x);
    const maxHeight = coordinates.reduce((max, point) => Math.max(max, point.y), coordinates[0].y) - coordinates.reduce((min, point) => Math.min(min, point.y), coordinates[0].y);
    function drawText(text, x, y) {
        const words = text.split('');
        let line = '';
        let lines = [];
        let lineHeight = 20;
        let totalHeight = 0;
        words.forEach(word => {
            const testLine = line + word;
            const testWidth = ctx.measureText(testLine).width;

            if (testWidth > maxWidth) {
                lines.push(line);
                line = word;
                totalHeight += lineHeight;
            } else {
                line = testLine;
            }
        });
        lines.push(line);
        totalHeight += lineHeight;

        // テキストの総高さがmaxHeightを超える場合、フォントサイズを調整
        if (totalHeight > maxHeight){
            let fontSize = parseInt(ctx.font.match(/\d+/), 10);
            // 最小フォントサイズを5pxに設定
            while (totalHeight > maxHeight && fontSize > 5){
                fontSize--;
                ctx.font = `bold ${fontSize}px Arial`;
                // フォントサイズに基づいて行の高さを再計算
                lineHeight = fontSize * 1.2;
                totalHeight = lines.length * lineHeight;
            }
        }
        // テキストを中央に描画
        const startY = y - (totalHeight / 2) + (lineHeight / 2);
        lines.forEach((line, index) => {
            ctx.fillText(line, x, startY + index * lineHeight);
        });
    }

    ctx.font = 'bold 10px Arial';
    ctx.fillStyle = 'black';
    ctx.textAlign = 'center';
    drawText(name, center.x, center.y);
}

// 矩形の中心を算出
function calculateCenter(coordinates) {
    let centerX = 0;
    let centerY = 0;
    for (let i = 0; i < coordinates.length; i++) {
        centerX += coordinates[i].x;
        centerY += coordinates[i].y;
    }

    centerX /= coordinates.length;
    centerY /= coordinates.length;
    return { x: centerX, y: centerY };
}

// 座標が矩形の内部にあるかを判定
function isInsidePolygon(x, y, coordinates) {
    let inside = false;
    let i, j = coordinates.length - 1;

    // 座標は既に補正済みなので、追加の補正は不要
    for (i = 0; i < coordinates.length; i++) {
        var ix = coordinates[i].x;
        var iy = coordinates[i].y;
        var jx = coordinates[j].x;
        var jy = coordinates[j].y;
        if ((iy > y) !== (jy > y) && (x < (jx - ix) * (y - iy) / (jy - iy) + ix)) {
            inside = !inside;
        }
        j = i;
    }
    return inside;
}

// マウス移動イベント
canvas.addEventListener('mousemove', function (event) {
    const mouseX = event.clientX - canvas.getBoundingClientRect().left;
    const mouseY = event.clientY - canvas.getBoundingClientRect().top;

    sectionDatas.forEach(function (section) {
        const sectionCoords = coordDatas?.find(data => data["SectionCode"] == section.sectionCode);
        // 空きあり場合
        if (sectionCoords && section.noReserveCount > 0) {
            //const sectionCoords = coordDatas.find(data => data["SectionCode"] == section.sectionCode);
            sectionCoords.Coordinates.forEach(function (coords) {
                // 矩形と名前を描画
                const isInside = isInsidePolygon(mouseX, mouseY, coords);
                ctx.strokeStyle = isInside ? 'red' : 'black';
                drawRect(coords, section.noReserveCount);
                drawName(coords, section.sectionName);
            });
        }
    });
});

// クリック/タップイベントの統一処理
function handlePointerEvent(event) {
    //event.preventDefault(); // デフォルトの動作を防ぐ
    
    const rect = canvas.getBoundingClientRect();
    let clickX, clickY;
    
    // タッチイベントの場合
    if (event.type === 'touchend' || event.type === 'touchstart') {
        const touch = event.changedTouches[0];
        clickX = touch.clientX - rect.left;
        clickY = touch.clientY - rect.top;
    } else {
        // マウスイベントの場合
        clickX = event.clientX - rect.left;
        clickY = event.clientY - rect.top;
    }
    
    // Canvas のスケールを考慮した座標補正
    const scaleX = canvas.width / rect.width;
    const scaleY = canvas.height / rect.height;
    clickX = clickX * scaleX;
    clickY = clickY * scaleY;

    for (let i = 0; i < sectionDatas.length; i++) {
        const section = sectionDatas[i];
        const sectionCoords = coordDatas?.find(data => data["SectionCode"] == section.sectionCode);
        // 空きあり場合
        if (sectionCoords && section.noReserveCount > 0) {
            //const sectionCoords = coordDatas.find(data => data["SectionCode"] == section.sectionCode);
            sectionCoords.Coordinates.forEach(function (coords) {
                // 矩形選択
                const isInside = isInsidePolygon(clickX, clickY, coords);
                if (isInside) {
                    window.location.href = "/PlotDetails?Index=" + section.sectionIndex;
                    return; // 最初にヒットした区画で処理を終了
                }
            });
        }
    }
}

// マウスクリックイベント
canvas.addEventListener('click', handlePointerEvent);

// タッチイベント（スマートフォン対応）
canvas.addEventListener('touchend', handlePointerEvent);

//// ダブルタップによるズームを防ぐ
//canvas.addEventListener('touchstart', function(event) {
//    if (event.touches.length > 1) {
//        event.preventDefault();
//    }
//});
