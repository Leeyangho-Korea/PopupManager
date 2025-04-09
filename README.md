# YH Popup Manager

유니티 Addressables + 오브젝트 풀링을 기반으로 동작하는 확장 가능한 팝업 시스템입니다.  
BasePopup 상속 구조와 에디터 자동화 기능을 통해 팝업 UI 개발 효율을 극대화합니다.

---

## 주요 기능

### Addressables 기반 팝업 로딩
- 프리팹을 Addressables에 등록하고, Address 키를 통해 비동기 로딩

### 오브젝트 풀링
- 생성된 팝업은 숨겨진 채로 유지되며 재사용되어 성능 최적화

### UI 정렬 보장
- 새로 띄운 팝업은 `SetAsLastSibling()`을 통해 항상 UI 최상단에 표시됨

### 팝업 스택 + ESC 키 제어
- 활성화된 팝업은 스택에 저장되며, ESC 키 입력 시 가장 최근 팝업만 닫힘
- `IsEscapeClosable` 속성으로 ESC 반응 여부 제어 가능

### 버튼-액션 키 매핑 구조
- 버튼과 string 키(`actionKey`)를 매핑한 뒤, `switch`문 기반 액션 처리 가능

### 에디터 자동화
- 팝업 스크립트 자동 생성
- 버튼-키 매핑 기반 `HandleAction` 메서드 템플릿 자동 생성

---

## 사용 방법

### 1. Addressables 설정

#### 1-1. 팝업 프리팹 등록
- Project 창에서 팝업 프리팹 선택
- 인스펙터에서 **Addressables 체크**
- **Address 값**을 `"InfoPopup"`처럼 설정 (코드에서 이 키로 접근함)

#### 1-2. Addressables 빌드
- 메뉴: `Window > Asset Management > Addressables > Build > New Build > Default Build Script`

---

### 2. 팝업 프리팹 설정

#### 2-1. `Popup.cs` 컴포넌트 추가
- 팝업 프리팹에 `Popup` 스크립트 추가

#### 2-2. 자동 스크립트 생성
- 인스펙터에서 `"스크립트 자동 생성 및 할당"` 버튼 클릭
- 프리팹 이름(`InfoPopup`)과 동일한 스크립트가 자동 생성됨
- `BasePopup`을 상속하며 기본 `HandleAction()` 포함됨

---

### 3. 버튼-키 매핑

#### 3-1. `BasePopup`의 인스펙터 필드
- `buttonMappings` 리스트에 다음 항목 입력
  - Button: 실제 버튼 오브젝트
  - actionKey: 처리할 키워드 (`"Confirm"`, `"Cancel"` 등)

#### 3-2. 자동 switch문 생성
- `"HandleAction switch문 복사"` 버튼을 누르면  
  등록된 키를 기반으로 switch문이 생성되어 클립보드에 복사됨

---

### 4. 팝업 코드 예시

```csharp
public class InfoPopup : BasePopup
{
    protected override void HandleAction(string key)
    {
        switch (key)
        {
            case "Confirm":
                Debug.Log("확인 버튼 클릭됨");
                Close();
                break;
            case "Cancel":
                Debug.Log("취소 버튼 클릭됨");
                Close();
                break;
        }
    }
}
```
---

### 5. 호출 방법

```csharp
PopupManager.Instance.ShowPopup<InfoPopup>("InfoPopup", popup =>
{
    // 추가 초기화 코드 작성 가능
});

```
#### 5-1. 호출 예시

```charp
  void ShowPopup(string popupName)
 {
     PopupManager.Instance.ShowPopup<BasePopup>(popupName, popup =>
     {
         Debug.Log($"[STACK TEST] {popupName} 팝업 생성됨");
     });
 }
```

### 6. ESC 키로 팝업 닫기

- 활성화된 팝업은 내부적으로 스택에 쌓이며, ESC 키 입력 시 가장 마지막에 띄운 팝업부터 닫힙니다.
- ESC 입력은 `PopupManager`가 중앙에서 감지하며, `BasePopup`의 `IsEscapeClosable` 속성으로 반응 여부를 제어할 수 있습니다.

#### 6-1. 예시: ESC로 닫히지 않도록 설정

```csharp
public class ExaamplePopup : BasePopup
{
    public override bool IsEscapeClosable => false;
}
```
