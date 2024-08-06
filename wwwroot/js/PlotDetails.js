function printCemeteryInfo(cemeteryCode) {
    var cemeteryContainer = document.getElementById('table-container-' + cemeteryCode);
    if (cemeteryContainer) {
        var districtNameElement = cemeteryContainer.querySelector('.district-name');
        var imageContentElement = cemeteryContainer.querySelector('.image-content');
        var dataTableElement = cemeteryContainer.querySelector('.data-table');
        var contactInfoElement = cemeteryContainer.querySelector('.contact-info');
        var buttonContainerElement = cemeteryContainer.querySelector('.button-container');
        var reserveLabelElements = Array.from(cemeteryContainer.querySelectorAll('label'));
        var phoneDivElement = cemeteryContainer.querySelector('div > a[href^="tel:"]').parentNode;

        // 各要素の内容を取得し、存在しない場合は空の文字列を設定
        var districtName = districtNameElement ? districtNameElement.innerHTML : '';
        var imageContent = imageContentElement ? imageContentElement.innerHTML : '';
        var dataTable = dataTableElement ? dataTableElement.outerHTML : '';
        var contactInfo = contactInfoElement ? contactInfoElement.outerHTML : '';

        // ボタンコンテナから「印刷する」ボタンを除外
        var buttonContainer = buttonContainerElement ? buttonContainerElement.cloneNode(true) : null;
        if (buttonContainer) {
            var printButton = buttonContainer.querySelector('.button-print');
            if (printButton) {
                printButton.parentNode.removeChild(printButton);
            }
        }

        var reserveLabels = reserveLabelElements.map(label => label.outerHTML).join('');
        var phoneDivHTML = phoneDivElement ? phoneDivElement.outerHTML : '';

        var printWindow = window.open('', '_blank', 'width=800,height=600');
        printWindow.document.write('<html><head><title>印刷</title>');
        printWindow.document.write('<link href="/css/PlotDetails.css?v=' + Date.now() + '" rel="stylesheet" type="text/css" />');
        printWindow.document.write('<style>');
        printWindow.document.write('@media print { .qr-code { display: block !important; } .image-content { gap: 0 !important; justify-content: space-around !important; } .image-container { margin: 0 !important; width: 48% !important; } .image-container img { max-height: 300px !important; margin: 0 !important; } .image-container .caption { display: block !important; margin: 0; padding: 0; } }');
        printWindow.document.write('@media screen { .qr-code { display: none; } }');
        printWindow.document.write('body { font-size: 12pt; line-height: 1.5; } .PlotInfoContainer { border: 1px solid #000; padding: 10px; margin: 10px 0; } .district-name { font-size: 16pt; font-weight: bold; margin-bottom: 10px; } .image-content { display: flex; justify-content: space-between; margin-bottom: 10px; } .image-container { display: flex; flex-direction: column; align-items: center; } .image-container img { max-width: 100%; height: auto; border: 1px solid #000; max-height: 300px; margin-bottom: 0; } .image-container .caption { font-size: 10pt; text-align: center; margin-top: 5px; margin-bottom: 0; } .data-table { width: 100%; border-collapse: collapse; margin-bottom: 10px; margin-left: auto; margin-right: auto; } .data-table th, .data-table td { border: 1px solid #000; padding: 5px; text-align: left; } .data-table th { background-color: #f0f0f0; } .data-table td { background-color: #fff; } .button-container { display: block; text-align: center; margin-top: 20px; } .contact-info { text-align: center; margin-top: 20px; } .contact-info img { display: inline-block; vertical-align: middle; } .qr-code { position: fixed; bottom: 10px; left: 50%; transform: translateX(-50%); display: none; } @page { margin: 0; } body { margin: 1cm; }');
        printWindow.document.write('</style>');
        printWindow.document.write('</head><body>');
        printWindow.document.write('<div class="PlotInfoContainer">');
        printWindow.document.write('<p class="district-name">' + districtName + '</p>');
        printWindow.document.write('<div class="image-content">' + imageContent.replace(dataTableElement.outerHTML, '') + '</div>'); // データテーブルを除外
        printWindow.document.write(dataTable); // データテーブルをここで表示
        printWindow.document.write(reserveLabels); // 印刷用に表示
        if (buttonContainer) {
            printWindow.document.write(buttonContainer.outerHTML); // 印刷用に表示
        }
        printWindow.document.write(contactInfo); // 印刷用に表示
        printWindow.document.write(phoneDivHTML); // 印刷用に表示
        printWindow.document.write('<div class="qr-code"><img src="https://api.qrserver.com/v1/create-qr-code/?size=100x100&data=https://yasiro.jp" alt="QR Code"></div>'); // QRコードを表示
        printWindow.document.write('</div>');
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.onload = function () {
            printWindow.print();
            printWindow.close();
        };
    } else {
        alert('指定された墓所情報が見つかりませんでした。');
    }
}

var map = null; // 地図データ
// jsonファイル読込3
let coordDatas;
let dispWidth;
let dispHeight;
document.addEventListener('DOMContentLoaded', () => {
    var areaFile = '/data/SEC_' + ReienCode + '_' + AreaCode + '_' + SectionCode + '.json' + '?v=' + new Date().getTime();
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
            // 表示範囲データの読込
            dispWidth = coordDatas[0].displayWidth;
            dispHeight = coordDatas[0].displayHeight;

            // 初期表示
            initMap();
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
});

// マップ初期表示
var corrRate = 1.0;
function initMap() {
    var imagePath = '/images/SEC_' + ReienCode + '_' + AreaCode + '_' + SectionCode + '.png';
    var img = new Image();
    img.src = imagePath;
    img.onload = function () {
        // 画像サイズ
        var imageWidth = img.width;
        var imageHeight = img.height;

        // 実際の表示範囲
        var displayWidth = dispWidth;
        var displayHeight = dispHeight;

        map = L.map('map-container', {
            crs: L.CRS.Simple,
            zoomControl: true,
            maxZoom: 1.2
        });

        // マップコンテナのサイズを実際の表示範囲に合わせる
        var mapContainer = document.getElementById('map-container');
        var mapWidth = mapContainer.offsetWidth;
        var mapHeight = mapContainer.offsetHeight;
        if (mapWidth <= imageWidth) {
            corrRate = mapWidth / imageWidth;
        }
        mapContainer.style.width = (displayWidth * corrRate) + 'px';
        mapContainer.style.height = (displayHeight * corrRate) + 'px';

        // 画像の実際のサイズ
        var displayBounds = [[0, 0], [displayHeight, displayWidth]];
        var imageBounds = [[0, 0], [imageHeight * corrRate, imageWidth * corrRate]];
        var moveBounds = [[(imageHeight - displayHeight) * corrRate, (imageWidth - displayWidth) * corrRate], [imageHeight * corrRate, imageWidth * corrRate]];
        var image = L.imageOverlay(imagePath, imageBounds).addTo(map);

        // 左上を起点に設定
        map.setView([0, 0], 0);
        map.setMaxBounds(moveBounds);

        map.on('resize', function () {
            map.setView([0, 0], 0);
        });

        var polygons = [];
        var activePolygon = null;

        // 墓所情報
        CemeteryDatas.forEach(function (cemetery) {
            // 墓所矩形情報
            const cemeteryCoords = coordDatas.find(data => data["CemeteryCode"] == cemetery.cemeteryCode);
            createPolygon(cemeteryCoords);
        });

        // QRコードから遷移の場合
        if (InitCemeteryCode != "") {
            // 墓所情報の表示
            const element = document.getElementById('table-container-' + InitCemeteryCode);
            if (element) {
                element.style.display = 'block';
                element.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        }

        // ↓↓↓ 画像作成用処理（通常コメント化）↓↓↓
        //coordDatas.forEach(function (coordData) {
        //    createPolygon(coordData);
        //});
        // ↑↑↑ 画像作成用処理（通常コメント化）↑↑↑

        // 矩形を描画
        function createPolygon(cemetery) {
            const coordinates = cemetery.Coordinates[0].map(coord => [(coord.y * corrRate), (coord.x * corrRate)]);
            var polygon = L.polygon(coordinates, {
                color: 'black',
                fillColor: 'yellow',
                fillOpacity: 0.5,
                weight: 1,
                polygonId: 'table-container-' + cemetery.CemeteryCode
            }).addTo(map);
            polygon.on('click', onPolygonClick);
            polygons.push(polygon);

            // QRコードから遷移の場合
            if (cemetery.CemeteryCode == InitCemeteryCode) {
                // 墓所矩形の選択
                var event = {
                    latlng: polygon.getBounds().getCenter(),
                    target: polygon
                };
                polygon.fire('click', event);
            }
        }

        // 矩形のクリックイベント
        function onPolygonClick(e) {
            var clickedLatLng = e.latlng;
            var isInsidePolygon = e.target.getBounds().contains(clickedLatLng);

            if (activePolygon) {
                activePolygon.setStyle({ fillColor: 'yellow' });
            }

            activePolygon = e.target;
            activePolygon.setStyle({ fillColor: 'red' });

            var polygonId = e.target.options.polygonId;
            showTable(polygonId);
        }

        // 墓所情報の表示を描画
        function showTable(id) {
            document.querySelectorAll('div[id^="table-container"]').forEach(div => div.style.display = 'none');
            var table = document.getElementById(id);
            if (table) {
                table.style.display = 'block';
                scrollToTable(id);
            }
        }

        // スクロール設定
        function scrollToTable(tableId) {
            var table = document.getElementById(tableId);
            if (table) {
                var tablePosition = table.getBoundingClientRect().top + window.pageYOffset;
                window.scrollTo({ top: tablePosition, behavior: 'smooth' });
            }
        }
    };
}
