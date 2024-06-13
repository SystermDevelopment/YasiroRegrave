document.addEventListener('DOMContentLoaded', function () {
    var filterReien = document.getElementById('filter-reien');
    var filterArea = document.getElementById('filter-area');
    var filterSection = document.getElementById('filter-section');

    // 霊園リストボックスの変更イベント
    document.getElementById('filter-reien').addEventListener('change', function () {
        // エリア・区画リストボックスの初期化
        document.getElementById('filter-area').selectedIndex = 0;
        document.getElementById('filter-section').selectedIndex = 0;
        // リストの更新
        updateAreaList();
        updateSectionList();
    });

    // エリアリストボックスの変更イベント
    document.getElementById('filter-area').addEventListener('change', function () {
        // 区画リストボックスの初期化
        document.getElementById('filter-section').selectedIndex = 0;
        // リストの更新
        updateSectionList();
    });

    // エリアリストの更新
    function updateAreaList() {
        var selectedReienIndex = filterReien.value;
        var selectedAreaIndex = filterArea.value;
        filterArea.innerHTML = '';

        // 霊園リストボックスが未選択のとき無効
        if (selectedReienIndex == '' || selectedReienIndex == '-1') {
            filterReien.value = -1;
            filterArea.value = -1;
            filterArea.selectedIndex = -1;
            filterArea.disabled = true;
            return;
        } else {
            filterArea.disabled = false;
        }

        // 未選択のオプションを追加
        var defaultOption = document.createElement('option');
        defaultOption.value = '-1';
        defaultOption.text = '-';
        filterArea.appendChild(defaultOption);

        // 選択された霊園に対応するエリアのリストを取得
        var areasForReien = Areas.filter(function (area) {
            return area.reienIndex == selectedReienIndex;
        });

        // エリアリストボックスに選択肢を追加
        areasForReien.forEach(function (area) {
            var option = document.createElement('option');
            option.value = area.areaIndex;
            option.text = area.areaName;
            filterArea.appendChild(option);
        });

        // POST時のみ再設定
        if (selectedAreaIndex != '' && selectedAreaIndex != '-1') {
            filterArea.value = selectedAreaIndex
        }
    }

    // 区画リストの更新
    function updateSectionList() {
        var selectedAreaIndex = filterArea.value;
        var selectedSectionIndex = filterSection.value;
        filterSection.innerHTML = '';

        // エリアリストボックスが未選択のとき無効
        if (selectedAreaIndex == '' || selectedAreaIndex == '-1') {
            filterSection.selectedIndex = -1;
            filterSection.disabled = true;
            return;
        } else {
            filterSection.disabled = false;
        }

        // 未選択のオプションを追加
        var defaultOption = document.createElement('option');
        defaultOption.value = '-1';
        defaultOption.text = '-';
        filterSection.appendChild(defaultOption);

        // 選択されたエリアに対応する区画のリストを取得
        var sectionsForArea = Sections.filter(function (sect) {
            return sect.areaIndex == selectedAreaIndex;
        });

        // 区画リストボックスに選択肢を追加
        sectionsForArea.forEach(function (sect) {
            var option = document.createElement('option');
            option.value = sect.sectionIndex;
            option.text = sect.sectionName;
            filterSection.appendChild(option);
        });

        // POST時のみ再設定
        if (selectedSectionIndex != '' && selectedSectionIndex != '-1') {
            filterSection.value = selectedSectionIndex
        }
    }

    // 初期表示時にリストを更新
    updateAreaList();
    updateSectionList();
});