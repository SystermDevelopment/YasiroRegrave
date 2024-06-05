// 多角形の座標情報
const trapezoidCoordinates = [
    [509, 275, 509, 341, 595, 342, 540, 275, "日蓮D区"],
    [315, 135, 389, 135, 389, 262, 315, 262, "親鸞E区"],
    [136, 347, 308, 347, 308, 402, 136,402, "釈尊D区"],
    [313, 347, 491, 347, 491, 405, 313, 405, "親鸞C区"],
    [124, 415, 309, 415, 309, 499, 124, 499, "釈尊C区"],
    [514, 348, 514, 403, 662, 403, 609, 348, "日蓮C区"],
    [325, 415, 497, 415, 497, 485, 325, 485, "親鸞C区"],
    [504, 485, 690, 485, 690, 551, 504, 551, "日蓮B区"],
    [326, 487, 497, 487, 497, 552, 326, 552, "親鸞B区"],
    [217, 501, 309, 501, 309, 563, 217, 563, "釈尊特A区"],
    [103, 97, 125, 97, 125, 269, 103, 269, "釈尊新区"],
    [503, 419, 690, 419, 690, 483, 503, 483, "日蓮C区"]
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

// クリックイベントの処理
canvas.addEventListener('click', function (event) {
    handleEvent(event.clientX, event.clientY);
});

// タッチイベントの処理
canvas.addEventListener('touchstart', function (event) {
    const touch = event.touches[0];
    handleEvent(touch.clientX, touch.clientY);
});

function handleEvent(clientX, clientY) {
    const rect = canvas.getBoundingClientRect();
    const clickX = clientX - rect.left;
    const clickY = clientY - rect.top;

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
                    reservationPageUrl = "https://localhost:7147/PlotDetails";
                    break;
            }
            if (reservationPageUrl !== "") {
                window.location.href = reservationPageUrl;
            }
        }
    });
}