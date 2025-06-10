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





---
좋아! 지금까지 보여준 세 개 코드 (`MainWindow`, `Person` 클래스, `AddPerson` 창)를 바탕으로, **프로그램이 실행되고 사용자와 상호작용할 때 코드가 어떻게 동작하는지 순서대로 전체 흐름**을 쉽게 설명해줄게.

---

# 주소록 프로그램 동작 흐름 정리

---

## 1. 프로그램 시작

* WPF 앱 실행 → `MainWindow`가 열림 (`MainWindow` 클래스의 생성자 실행)
* 생성자에서

  * UI 컴포넌트 초기화 (`InitializeComponent()`)
  * `people` 컬렉션과 `PersonInfo` DataGrid를 연결 (`PersonInfo.ItemsSource = people`)
  * 검색 조건 ComboBox에 `SearchList` 바인딩 (`InfoList.ItemsSource = SearchList`)
  * 데이터 파일(`data.txt`)에서 기존 저장된 사람 목록 읽기 (`LoadDataFromFile()`)
* 읽은 데이터는 `Person` 객체 리스트로 변환 후 `people`에 추가 → UI에 목록 표시됨

---

## 2. 사용자 인터페이스 주요 기능

---

### (1) 사람 추가하기

* 사용자가 **추가 버튼** 클릭 → `Button_Click` 이벤트 발생
* `AddPerson` 창이 새로 열림 (기본 생성자 호출, 빈 입력란)
* 사용자가 정보를 입력하고 **추가 버튼** 누르면 (`AddButton_Click` 이벤트)

  * `AddPerson` 창 내에서 `NewPerson` 객체 생성 (입력 데이터 기반)
  * `DialogResult = true`로 창 닫힘
* `MainWindow`는 `ShowDialog()` 결과가 `true`면

  * `NewPerson`을 `people` 컬렉션에 추가
  * `SaveDataToFile()` 호출해 파일에 새 목록 저장
* UI(DataGrid) 자동 갱신됨 (ObservableCollection 덕분)

---

### (2) 사람 수정하기

* 사용자가 DataGrid에서 한 사람 선택 → `Edit_Click` 버튼 클릭
* `AddPerson` 창이 열림 (수정 생성자 호출, 선택된 사람 정보가 입력란에 미리 채워짐)
* 사용자가 내용을 수정하고 **추가/저장 버튼** 누르면

  * `NewPerson` 객체가 새 값으로 생성됨
  * 창 닫힘(`DialogResult = true`)
* `MainWindow`는 선택된 `Person` 객체 속성을 새 값으로 일일이 업데이트
* DataGrid 갱신 (`Items.Refresh()`) 후 `SaveDataToFile()` 호출해 저장

---

### (3) 사람 삭제하기

* 사용자가 DataGrid에서 한 사람 선택 → `Delete_Click` 버튼 클릭
* 삭제 확인 메시지 박스 출력
* 사용자 확인하면 `people`에서 해당 사람 제거
* `SaveDataToFile()` 호출해 변경사항 저장
* DataGrid 자동 갱신

---

### (4) 검색 기능

* 사용자가 검색 조건(전체, 이름, 소속, 직급) 선택

  * ‘전체’ 선택 시 검색창 비활성화, 전체 리스트 표시
  * 특정 조건 선택 시 검색창 활성화
* 검색어 입력 후 **검색 버튼 클릭** 또는 **엔터키 누름**
* `SearchButton_Click` 실행:

  * ‘전체’면 전체 `people` 리스트 보여주고,
  * 조건별로 `people` 리스트에서 필터링한 결과만 DataGrid에 보여줌

---

## 3. 데이터 저장 및 불러오기

* 모든 추가, 수정, 삭제 작업 후 `SaveDataToFile()` 호출됨

  * `people` 컬렉션의 각 `Person`을 `ToString()`으로 문자열 변환 (구분자 `|`로 연결)
  * 전체 리스트를 `data.txt` 파일에 덮어쓰기 저장
* 프로그램 시작 시 `LoadDataFromFile()`가 파일에서 모든 라인을 읽어와

  * 각 줄을 `Person.FromString()`으로 파싱해 `people`에 추가

---

# 핵심 요약

| 단계         | 동작                                              | 주요 코드 위치/핵심 메서드                             |                                          |
| ---------- | ----------------------------------------------- | ------------------------------------------- | ---------------------------------------- |
| 프로그램 시작    | 파일에서 기존 데이터 읽어와 UI에 표시                          | `MainWindow()` 생성자, `LoadDataFromFile()`    |                                          |
| 사람 추가      | `AddPerson` 창 띄워 입력받고, 결과를 리스트에 추가 후 파일 저장      | `Button_Click`, `AddPerson.AddButton_Click` |                                          |
| 사람 수정      | 선택된 사람 정보로 `AddPerson` 창 띄워 수정 후 리스트에 반영, 파일 저장 | `Edit_Click`, `AddPerson.AddButton_Click`   |                                          |
| 사람 삭제      | 선택된 사람 리스트에서 제거 후 파일 저장                         | `Delete_Click`                              |                                          |
| 검색 기능      | 조건과 키워드로 리스트 필터링 후 UI 갱신                        | `SearchButton_Click`                        |                                          |
| 데이터 파일 입출력 | 리스트 ↔ 텍스트 파일 (구분자 \`                            | \` 문자열 변환)                                  | `SaveDataToFile()`, `LoadDataFromFile()` |

---

이 흐름대로 실행되니, 사용자는 주소록을 편리하게 관리할 수 있고, 변경사항은 파일에 안전하게 저장돼 다음 실행 때도 유지되는 거야.

---

필요하면 UI XAML 구조도 같이 보면서 UI 이벤트가 어떻게 연결되는지 설명해줄 수 있어. 더 궁금한 부분 있으면 알려줘!

