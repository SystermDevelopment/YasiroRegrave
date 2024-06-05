const canvas = document.getElementById('plotCanvas');   // canvas要素を取得
const ctx = canvas.getContext('2d');                    // 描画コンテキストを取得

$(function () {
    // 矩形と名前を描画
    sectionDatas.forEach(function (section) {
        if (section.noReserveCount > 0) {
            drawRect(section.coordinates);
            drawName(section.coordinates, section.sectionName);
            ctx.strokeStyle = 'black';
            ctx.stroke();
        }
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
    ctx.fillStyle = 'rgb(255, 233, 7)';
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
        if (totalHeight > maxHeight) {
            let fontSize = parseInt(ctx.font.match(/\d+/), 10);
            // 最小フォントサイズを5pxに設定
            while (totalHeight > maxHeight && fontSize > 5) {
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
    for (i = 0; i < coordinates.length; i++) {
        if ((coordinates[i].y > y) !== (coordinates[j].y > y) &&
            x < (coordinates[j].x - coordinates[i].x) * (y - coordinates[i].y) / (coordinates[j].y - coordinates[i].y) + coordinates[i].x) {
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
        if (section.noReserveCount > 0) {
            const isInside = isInsidePolygon(mouseX, mouseY, section.coordinates);
            ctx.strokeStyle = isInside ? 'red' : 'black';
            drawRect(section.coordinates);
            drawName(section.coordinates, section.sectionName);
        }
    });
});

// マウスクリックイベント
canvas.addEventListener('click', function (event) {
    const clickX = event.clientX - canvas.getBoundingClientRect().left;
    const clickY = event.clientY - canvas.getBoundingClientRect().top;

    for (let i = 0; i < sectionDatas.length; i++) {
        const section = sectionDatas[i];
        if (section.noReserveCount > 0) {
            const isInside = isInsidePolygon(clickX, clickY, section.coordinates);
            if (isInside) {
                window.location.href = "/PlotDetails?Index=" + section.sectionIndex;
                break;
            }
        }
    }
});
