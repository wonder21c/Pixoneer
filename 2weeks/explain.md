물론이야! 너가 준 C# WPF 코드(주소록 관리 앱)는 기본적으로 주소록에 사람들 정보를 저장하고, 추가/수정/삭제하며, 검색하는 기능을 가진 프로그램이야.

---

## 코드 전체 구조와 주요 흐름 설명

### 1. `MainWindow` 클래스 개요

* WPF 앱의 메인 윈도우.
* 사람(Person) 리스트를 `ObservableCollection<Person>` 형태로 관리하며 UI의 DataGrid(아마 `PersonInfo`)에 바인딩해서 실시간 반영.
* 파일(`data.txt`)에서 데이터를 읽어와서 `people` 컬렉션에 넣고, 수정 시 파일에 저장.

### 2. 멤버 변수 및 속성

* `ObservableCollection<Person> people`: 동적으로 UI와 연동되는 사람 리스트.
* `string filePath`: 데이터를 저장/로드할 파일 경로.
* `List<string> Info`: UI에서 보여줄 컬럼명 리스트 (이름, 소속 등).
* `List<string> SearchList`: 검색 필터 리스트 (전체, 이름, 소속, 직급).

### 3. 생성자 `MainWindow()`

* UI 초기화(`InitializeComponent()`).
* `PersonInfo.ItemsSource = people`로 DataGrid와 `people` 컬렉션 연결.
* `InfoList.ItemsSource = SearchList`로 검색 조건 ComboBox에 항목 설정.
* `LoadDataFromFile()` 호출해 파일에서 기존 데이터 불러옴.

---

### 4. 주요 기능 메서드

#### a. `LoadDataFromFile()`

* 파일이 없으면 경고 메시지 표시 후 종료.
* 파일의 각 라인을 `Person.FromString()`으로 변환해 `people`에 추가.

#### b. `SaveDataToFile()`

* `people`의 모든 `Person` 객체를 `ToString()`으로 문자열화해서 파일에 한 줄씩 저장.

#### c. 추가 기능 - `Button_Click()`

* `AddPerson`이라는 별도 창(추가 폼)을 열어 사용자에게 새 사람 정보 입력받음.
* 완료되면 `people`에 추가, 파일 저장.

#### d. 수정 기능 - `Edit_Click()`

* DataGrid에서 선택된 `Person` 정보를 가져와서 수정 창 띄움.
* 수정 후 해당 `Person` 객체의 속성 직접 갱신하고 UI, 파일 저장.

#### e. 삭제 기능 - `Delete_Click()`

* 선택된 사람 삭제.
* 사용자 확인 메시지 후 삭제 및 파일 저장.

---

### 5. 검색 기능 - `SearchButton_Click()`

* 검색어와 검색 조건을 받아서 필터링.
* `전체` 선택 시 원래 `people` 리스트 보여주고,
* 특정 조건(이름, 소속, 직급) 선택 시 해당 조건에 맞는 사람들만 필터링해 DataGrid에 바인딩.

### 6. UI 이벤트 핸들러

* `Window_KeyDown`: 엔터키 누르면 검색 실행.
* `InfoList_SelectionChanged`: 검색 조건 변경 시, 전체 선택하면 검색창 비활성화, 다른 조건 선택하면 활성화.

---

## 정리

* **데이터 관리**: `ObservableCollection<Person>`으로 실시간 UI 갱신 지원.
* **파일 I/O**: 텍스트 파일에서 로드/저장.
* **CRUD**: 추가/수정/삭제 모두 UI와 파일에 반영.
* **검색**: 이름, 소속, 직급 기준으로 필터링.
* **UI 바인딩**: DataGrid, ComboBox, TextBox 등과 바인딩으로 편리한 UI 처리.

---


좋아! 이번에 올려준 `AddPerson` 클래스는 주소록에서 **새 사람 정보를 입력하거나 기존 사람 정보를 수정하는 창(Window)** 역할을 하는 WPF 윈도우야. 이 창의 동작과 설계 의도를 상세히 설명해줄게.

---

## `AddPerson` 클래스 개요

* WPF `Window`를 상속해서 만든 별도의 창.
* 사용자에게 사람(Person) 정보를 입력받거나, 이미 존재하는 정보를 보여줘서 수정 가능.
* 최종적으로 `NewPerson` 속성에 입력된 정보를 담아서 메인 윈도우에 전달.

---

## 주요 멤버와 메서드

### 1. 속성

```csharp
public Person NewPerson { get; set; }
```

* 창에서 입력된/수정된 사람 정보를 저장하는 프로퍼티.
* 외부에서 이 창을 `ShowDialog()`로 띄우고, 닫힐 때 이 속성을 읽어서 실제 데이터를 추가/수정에 활용.

---

### 2. 생성자

```csharp
public AddPerson()
{
    InitializeComponent();
}
```

* **새 사람 추가용 생성자.**
* UI 초기화만 수행하며, 모든 입력란은 빈 상태로 시작.

```csharp
public AddPerson(Person selectedPerson)
{
    InitializeComponent();
    if (selectedPerson != null)
    {
        NameBox.Text = selectedPerson.name;
        TeamBox.Text = selectedPerson.team;
        GradeBox.Text = selectedPerson.grade;
        PhoneBox.Text = selectedPerson.phoneNum;
        EmailBox.Text = selectedPerson.email;
    }
}
```

* **기존 사람 정보 수정용 생성자.**
* 전달받은 `selectedPerson` 객체를 바탕으로 각 입력 TextBox에 기존 데이터를 미리 채워 넣음.
* 사용자는 화면에 뜬 정보를 수정 가능.

---

### 3. 버튼 클릭 이벤트 핸들러

```csharp
private void AddButton_Click(object sender, RoutedEventArgs e)
{
    NewPerson = new Person
    {
        name = NameBox.Text,
        team = TeamBox.Text,
        grade = GradeBox.Text,
        phoneNum = PhoneBox.Text,
        email = EmailBox.Text
    };
    DialogResult = true;
    Close();
}
```

* 사용자가 ‘추가’ 또는 ‘저장’ 버튼(아마도 `AddButton`)을 누르면 실행됨.
* 각 입력란에서 텍스트를 읽어 새 `Person` 객체를 만들어 `NewPerson`에 할당.
* `DialogResult = true`로 모달 창에서 ‘성공’ 상태를 알리고 창을 닫음.
* 호출한 메인 윈도우는 `ShowDialog()` 반환값과 `NewPerson` 속성을 통해 사용자가 입력한 데이터를 받아서 처리.

---

## UI 컨트롤 (추정)

* `NameBox`, `TeamBox`, `GradeBox`, `PhoneBox`, `EmailBox` 는 각각 이름, 소속, 직급, 연락처, 이메일을 입력받는 **TextBox**.
* `AddButton`은 저장/추가 버튼으로, 클릭 시 데이터를 수집해 창을 닫음.

---

## 설계 의도와 장점

* **재사용성**: 추가와 수정 둘 다 한 창에서 처리. 생성자 오버로드로 구분.
* **간결함**: UI 입력을 그대로 `Person` 객체로 변환해 반환.
* **모달 방식**: `ShowDialog()`를 이용해 메인 윈도우가 창 닫힐 때까지 기다림. 사용자 입력 완료 후에만 데이터 처리.
* **명확한 결과 전달**: `DialogResult`와 `NewPerson`을 통해 호출자에게 성공 여부와 결과 데이터를 전달.

---

## 참고: 메모리/효율 측면

* UI 컨트롤(TextBox)의 텍스트를 한 번만 읽고 `Person` 객체를 만들기 때문에 불필요한 메모리 복사나 추가 처리가 없음.
* 창을 닫을 때까지 사용자 입력을 기다리므로, 데이터 유효성 검증 같은 추가 로직을 넣기도 쉽다 (현재 코드는 없음).

---

필요하면 UI XAML 코드도 보여줘. 그래야 어떤 버튼과 텍스트박스가 연결돼 있는지 더 명확히 설명해줄 수 있어. 궁금한 점 있으면 언제든 물어봐!

