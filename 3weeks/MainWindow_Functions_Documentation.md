
# 📘 `MainWindow.xaml.cs` 함수별 상세 문서

## ✅ `btnOpen_Click`
```csharp
private void btnOpen_Click(object sender, RoutedEventArgs e)
```

### ✔️ 기능 설명
사용자가 파일 열기 대화상자를 통해 `.ts` 파일을 선택하면, 선택한 각 파일을 `SourceList`에 `TargetInfo` 객체로 추가합니다.

### ✔️ 주요 처리
- `OpenFileDialog`를 사용하여 `.ts` 파일 다중 선택
- 각 선택된 파일 경로, 이름, 크기를 `TargetInfo`에 저장
- 완료 메시지박스 표시

### ✔️ 사용 예시
파일 목록을 트리뷰로 구성하고자 할 때, 소스 파일을 불러오는 진입점입니다.

---

## ✅ `btnConvert_Click`
```csharp
private async void btnConvert_Click(object sender, RoutedEventArgs e)
```

### ✔️ 기능 설명
체크된 `.ts` 파일에 대해 비동기 처리를 통해 `LoadVideoData()`를 실행하고, 각 파일을 CSV로 변환합니다.

### ✔️ 주요 처리
- `SourceList`에서 체크된 항목 필터링
- 항목별 `Task.Run()`을 통해 병렬 처리
- 처리 완료 후 메시지 박스 출력

### ✔️ 예외 처리
- 아무 파일도 선택되지 않은 경우 경고 메시지 출력

---

## ✅ `chkAllSelect_Checked / Unchecked`
```csharp
private void chkAllSelect_Checked(object sender, RoutedEventArgs e)
private void chkAllSelect_Unchecked(object sender, RoutedEventArgs e)
```

### ✔️ 기능 설명
- `chkAllSelect_Checked`: 모든 트리 항목의 체크박스를 **활성화**
- `chkAllSelect_Unchecked`: 모든 항목의 체크박스를 **비활성화**

### ✔️ 내부적으로 사용되는 함수
```csharp
SetIsCheckedRecursive(item, true/false);
```

---

## ✅ `SetIsCheckedRecursive`
```csharp
private void SetIsCheckedRecursive(TargetInfo item, bool isChecked)
```

### ✔️ 기능 설명
트리 구조의 모든 하위 항목에 대해 `IsChecked` 속성을 재귀적으로 적용합니다.

### ✔️ 사용 시나리오
전체 체크/해제 처리시 사용 (`chkAllSelect_Checked` 등에서 호출)

---

## ✅ `MakeTreeView_Source`
```csharp
void MakeTreeView_Source(TargetInfo _target)
```

### ✔️ 기능 설명
지정된 폴더 아래의 하위 폴더 및 `.ts` 파일을 재귀적으로 탐색하고 `TargetInfo`로 트리 구성합니다.

### ✔️ 처리 단계
1. `Directory.GetDirectories()`를 통해 하위 폴더 탐색
2. `Directory.GetFiles(...).Where(.ts)`를 통해 TS 파일 탐색
3. 탐색 결과를 `_target.Items`에 추가

---

## ✅ `LoadVideoData`
```csharp
void LoadVideoData(string _folderPath, string _filePath, TargetInfo target)
```

### ✔️ 기능 설명
`.ts` 파일을 `XVideoIO`를 이용해 열고 메타데이터(`MT_MV`)를 읽어온 후, CSV로 저장합니다. 또한 UI 진행률을 업데이트합니다.

### ✔️ 주요 단계
1. 파일 존재 확인
2. `XVideoIO.OpenFile`을 사용해 비디오 스트림 처리
3. 내부 콜백 함수에서 프레임마다 `MT_MV` 생성 및 수집
4. 일정 프레임마다 진행률 업데이트 (`Dispatcher.Invoke`)
5. 처리 완료 시 `GenerateMetadDataToCSV()` 호출

### ✔️ CSV 파일 경로 예시
```
{원본경로}/output/{파일명}.csv
```

---

## ✅ `GenerateMetadDataToCSV`
```csharp
void GenerateMetadDataToCSV(string _csvPath, List<MT_MV> metadList)
```

### ✔️ 기능 설명
`MT_MV` 객체의 속성값 중 `*_DP`로 끝나는 문자열 속성만 CSV로 추출하여 저장합니다.

### ✔️ 처리 방식
1. 리플렉션(`GetProperties`)을 사용하여 `*_DP` 문자열 속성 필터링
2. CSV 헤더 생성 (`_DP` 제거한 이름 사용)
3. 각 데이터 값을 `Escape()` 함수로 처리 후 CSV 라인 생성
4. `File.WriteAllText()`로 UTF-8로 저장

---

## ✅ `Escape`
```csharp
string Escape(string input)
```

### ✔️ 기능 설명
CSV 포맷에서 문제가 될 수 있는 문자(쉼표, 쌍따옴표, 줄바꿈 등)를 안전하게 이스케이프 처리합니다.

---

## ✅ `PreviewFrameMetad`
```csharp
private void PreviewFrameMetad(XVideoIO videoIO, int streamID, XFrameMetad data)
```

### ✔️ 기능 설명
`XVideoIO`의 메타데이터 콜백 함수에서 메타데이터를 받아 `listMetad`에 추가하는 용도입니다.

---

## ✅ `GetFilesByExtension`
```csharp
public List<string> GetFilesByExtension(string folderPath, string extension)
```

### ✔️ 기능 설명
지정한 폴더 및 하위 폴더에서 지정한 확장자를 가진 파일 경로 목록을 반환합니다.

---

# 📦 클래스 설명

## 🔸 `TargetInfo : TreeData`
- `TargetPath` : 파일/폴더 전체 경로
- `FileSize` : 바이트 단위 파일 크기
- `Progress` : 처리 진행률 (0~100)
- `FileSizeMb` : 사람이 읽을 수 있는 MB 단위 문자열

## 🔸 `TreeData` (Base Class)
- `Name`, `Level`, `IsChecked`
- `Items` : 자식 노드 목록
- `Add()`, `Remove()` : 노드 추가/삭제
- `RefreshAll()` : 바인딩 전체 새로고침
