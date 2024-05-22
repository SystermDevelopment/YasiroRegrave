// 多角形の座標情報
const trapezoidCoordinates = [
    [786, 274, 787, 341, 874, 341, 818, 274, "日蓮D区"],
    [592, 135, 666, 135, 666, 262, 592, 262, "親鸞E区"],
    [413, 349, 586, 349, 586, 401, 413, 401, "釈尊D区"],
    [592, 350, 767, 350, 767, 402, 592, 402, "親鸞C区"],
    [403, 500, 494, 500, 494, 563, 403, 563, "釈尊A区"],
    [403, 415, 585, 415, 585, 499, 403, 499, "釈尊C区"],
    [781, 420, 966, 420, 966, 482, 781, 482, "日蓮C区"],
    [602, 416, 773, 416, 773, 484, 602, 484, "親鸞C区"],
    [780, 483, 965, 483, 965, 549, 780, 549, "日蓮B区"],
    [602, 485, 773, 485, 773, 549, 602, 549, "親鸞B区"],
    [495, 500, 584, 500, 584, 563, 495, 563, "釈尊特A区"],
    [381, 97, 401, 97, 401, 499, 381, 499, "釈尊新区"],
    [791, 346, 791, 403, 940, 402, 884, 347, "日蓮C区"]
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
    const lineHeight = 20;

    function drawText(text, x, y) {
        let line = '';
        let currentWidth = 0;

        if (ctx.measureText(text).width <= maxWidth) {
            ctx.fillText(text, x, y);
            return;
        }

        for (let i = 0; i < text.length; i++) {
            const charWidth = ctx.measureText(text[i]).width;
            if (currentWidth + charWidth > maxWidth && i > 0) {
                ctx.fillText(line, x, y);
                line = text[i];
                y += lineHeight;
                currentWidth = charWidth;
            } else {
                line += text[i];
                currentWidth += charWidth;
            }
        }
        ctx.fillText(line, x, y);
    }

    ctx.font = 'bold 14px Arial';
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
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "日蓮C区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "親鸞E区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "釈尊D区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "親鸞C区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "釈尊A区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "釈尊C区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "日蓮B区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "親鸞B区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "釈尊特A区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
                case "釈尊新区":
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
            }
            if (reservationPageUrl !== "") {
                window.location.href = reservationPageUrl;
            }
        }
    });
});
