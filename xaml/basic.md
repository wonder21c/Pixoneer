| **개념**              | **설명**                                            | **예시**                                     |
| ------------------- | ------------------------------------------------- | ------------------------------------------ |
| `Grid`              | 행과 열로 구성된 컨테이너. 자식 요소는 `Row`, `Column` 지정하여 위치시킴. | `<Grid>...</Grid>`                         |
| `RowDefinitions`    | 행을 정의하는 컬렉션. `Height` 속성으로 크기를 지정함.               | `<RowDefinition Height="Auto"/>`           |
| `ColumnDefinitions` | 열을 정의하는 컬렉션. `Width` 속성으로 크기를 지정함.                | `<ColumnDefinition Width="*"/>`            |
| `Grid.Row`          | 자식 요소가 배치될 행 번호 (0부터 시작).                         | `<Button Grid.Row="1"/>`                   |
| `Grid.Column`       | 자식 요소가 배치될 열 번호 (0부터 시작).                         | `<Button Grid.Column="2"/>`                |
| `Grid.RowSpan`      | 행 병합 (해당 행부터 몇 개 행 차지할지).                         | `<TextBox Grid.Row="0" Grid.RowSpan="2"/>` |
| `Grid.ColumnSpan`   | 열 병합.                                             | `<TextBox Grid.ColumnSpan="2"/>`           |

| **값**    | **설명**                 | **예시**                           |
| -------- | ---------------------- | -------------------------------- |
| `Auto`   | 자식 요소의 크기에 맞춤          | `<RowDefinition Height="Auto"/>` |
| `*` (별표) | 남은 공간을 비율로 나눔 (가변적 크기) | `<ColumnDefinition Width="*"/>`  |
| 숫자\*     | 여러 비율로 나눌 수 있음         | `<ColumnDefinition Width="2*"/>` |
| 고정값 (px) | 절대 크기로 지정              | `<RowDefinition Height="100"/>`  |


