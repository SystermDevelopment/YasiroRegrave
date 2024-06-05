document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('seatsCanvas');
    const ctx = canvas.getContext('2d');
    const img = new Image();
    img.src = 'images/akikukaku.png'; // 座席マップの画像をロード

    img.onload = function () {
        canvas.width = img.width;
        canvas.height = img.height;
        ctx.drawImage(img, 0, 0); // 画像をキャンバスに描画
    };

    // リサイズイベントで表示を切り替え
    window.addEventListener('resize', resizeCanvas);
    function resizeCanvas() {
        if (window.innerWidth < 600) {
            document.getElementById('fullMapContainer').style.display = 'none';
            document.getElementById('canvasContainer').style.display = 'block';
            updateCanvas();
        } else {
            document.getElementById('fullMapContainer').style.display = 'block';
            document.getElementById('canvasContainer').style.display = 'none';
        }
    }

    // キャンバスを更新する関数
    function updateCanvas() {
        canvas.width = img.width;
        canvas.height = img.height;
        ctx.drawImage(img, 0, 0);
    }

    // 初期表示時のリサイズ処理を実行
    resizeCanvas();

});