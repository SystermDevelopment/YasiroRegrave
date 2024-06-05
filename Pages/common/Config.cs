namespace YasiroRegrave.Pages.common
{
    public class Config
    {
        // 削除フラグ
        public enum DeleteType
        {
            未削除 = 0,
            削除 = 1
        }
        // 公開状態
        public enum ReleaseStatusType
        {
            準備中 = 0,
            販売中 = 1,
        }
        // 区画状態
        public enum SectionStatusType
        {
            空 = 0,
            WEB予約 = 1,
            拠点予約 = 2,
            成約 = 3,
        }
    }
}
