// 多角形の座標情報
const trapezoidCoordinates = [
    //[509, 275, 509, 341, 595, 342, 540, 275, "日蓮D区"],
    [135, 70, 176, 70, 176, 141, 135, 141, "親鸞E区"],
    [38, 186, 133, 186, 133, 216, 38, 216, "釈尊D区"],
    [136, 186, 231, 186, 231, 217, 136, 217, "親鸞C区"],
    [32, 223, 133, 223, 133, 268, 32, 268, "釈尊C区"],
    [237, 225, 339, 225, 339, 259, 237, 259, "日蓮C区"],
    [237, 261, 340, 261, 340, 297, 237, 297, "日蓮B区"],
    [140, 223, 235, 223, 235, 260, 140, 260, "親鸞C区"],
    [140, 262, 235, 262, 235, 297, 140, 297, "親鸞B区"],
    [82, 270, 134, 270, 134, 304, 82, 304, "釈尊特A区"],
    [21, 51, 33, 51, 33, 144, 21, 144, "釈尊新区"],
    [243, 186, 243, 216, 323, 216, 294, 186, "日蓮C区"]
];

const canvas = document.getElementById('plotCanvas'); // canvas要素を取得
const ctx = canvas.getContext('2d'); // 描画コンテキストを取得

// 図形と黒枠を描画する
trapezoidCoordinates.forEach(coord => {
    const [x1, y1, x2, y2, x3, y3, x4, y4, id] = coord;
    const dynamicPoints = [
        { x: x1, y: y1 },
        { x: x2, y: y2 },
        { x: x3, y: y3 },
        { x: x4, y: y4 }
    ];
    drawTrapezoid(dynamicPoints);
    drawId(dynamicPoints, id);
    ctx.strokeStyle = 'black'; // 赤い枠の色を設定
    ctx.stroke(); // 枠を描画

});


// 多角形を描画する関数

function drawTrapezoid(dynamicPoints) {
    ctx.beginPath();
    ctx.moveTo(dynamicPoints[0].x, dynamicPoints[0].y);
    for (let i = 1; i < dynamicPoints.length; i++) {
        ctx.lineTo(dynamicPoints[i].x, dynamicPoints[i].y);
    }
    ctx.closePath();
    ctx.fillStyle = 'rgb(255, 233, 7)';
    ctx.fill();
    ctx.stroke();
}

// IDを描画する関数
function drawId(dynamicPoints, id) {
    const center = calculateCenter(dynamicPoints);
    const maxWidth = dynamicPoints.reduce((max, point) => Math.max(max, point.x), dynamicPoints[0].x) - dynamicPoints.reduce((min, point) => Math.min(min, point.x), dynamicPoints[0].x);
    const maxHeight = dynamicPoints.reduce((max, point) => Math.max(max, point.y), dynamicPoints[0].y) - dynamicPoints.reduce((min, point) => Math.min(min, point.y), dynamicPoints[0].y);

    function drawText(text, x, y) {
        const words = text.split('');
        let line = '';
        let lines = [];
        let lineHeight = 20; // 行の高さを設定
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
        lines.push(line); // 最後の行を追加
        totalHeight += lineHeight;

        // テキストの総高さがmaxHeightを超える場合、フォントサイズを調整
        if (totalHeight > maxHeight) {
            let fontSize = parseInt(ctx.font.match(/\d+/), 10);
            while (totalHeight > maxHeight && fontSize > 5) { // 最小フォントサイズを5pxに設定
                fontSize--;
                ctx.font = `bold ${fontSize}px Arial`;
                lineHeight = fontSize * 1.2; // フォントサイズに基づいて行の高さを再計算
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
    drawText(id, center.x, center.y);
}

// 多角形の中心を計算する関数
function calculateCenter(dynamicPoints) {
    let centerX = 0;
    let centerY = 0;
    for (let i = 0; i < dynamicPoints.length; i++) {
        centerX += dynamicPoints[i].x;
        centerY += dynamicPoints[i].y;
    }
    centerX /= dynamicPoints.length;
    centerY /= dynamicPoints.length;
    return { x: centerX, y: centerY };
}

// 座標が多角形の内部にあるかどうかを判定する関数
function isInsidePolygon(x, y, dynamicPoints) {
    let inside = false;
    let i, j = dynamicPoints.length - 1;
    for (i = 0; i < dynamicPoints.length; i++) {
        if ((dynamicPoints[i].y > y) !== (dynamicPoints[j].y > y) &&
            x < (dynamicPoints[j].x - dynamicPoints[i].x) * (y - dynamicPoints[i].y) / (dynamicPoints[j].y - dynamicPoints[i].y) + dynamicPoints[i].x) {
            inside = !inside;
        }
        j = i;
    }
    return inside;
}

// マウスが動くたびにカーソルの位置で多角形の範囲内にあるかどうかを判定し、外枠の色を変更する
canvas.addEventListener('mousemove', function (event) {
    const mouseX = event.clientX - canvas.getBoundingClientRect().left;
    const mouseY = event.clientY - canvas.getBoundingClientRect().top;

    trapezoidCoordinates.forEach(coord => {
        const [x1, y1, x2, y2, x3, y3, x4, y4, id] = coord;
        const dynamicPoints = [
            { x: x1, y: y1 },
            { x: x2, y: y2 },
            { x: x3, y: y3 },
            { x: x4, y: y4 }
        ];

        const isInside = isInsidePolygon(mouseX, mouseY, dynamicPoints);
        ctx.strokeStyle = isInside ? 'red' : 'black';
        drawTrapezoid(dynamicPoints);
        drawId(dynamicPoints, id);
    });
});


// マウスがクリックされたときの処理
canvas.addEventListener('click', function (event) {
    const clickX = event.clientX - canvas.getBoundingClientRect().left;
    //event.clientX は、ブラウザウィンドウの左端を基準としたマウスポインターの X 座標です。
    // この値からキャンバス要素の左端のオフセットを引いて、キャンバス内の相対的な X 座標を取得しています。
    const clickY = event.clientY - canvas.getBoundingClientRect().top;//Yも同様

    trapezoidCoordinates.forEach(coord => {
        const [x1, y1, x2, y2, x3, y3, x4, y4, id] = coord;
        const dynamicPoints = [
            { x: x1, y: y1 },
            { x: x2, y: y2 },
            { x: x3, y: y3 },
            { x: x4, y: y4 }
        ];

        const isInside = isInsidePolygon(clickX, clickY, dynamicPoints);
        if (isInside) {
            let reservationPageUrl = "";

            switch (id) {
                case "日蓮D区":
                case "日蓮C区":
                case "親鸞E区":
                case "釈尊D区":
                case "親鸞C区":
                case "釈尊C区":
                case "日蓮B区":
                case "親鸞B区":
                case "釈尊特A区":
                case "釈尊新区":
                    reservationPageUrl = "/PlotDetails";
                    break;
            }

            if (reservationPageUrl !== "") {
                window.location.href = reservationPageUrl;
            }
        }
    });
});
