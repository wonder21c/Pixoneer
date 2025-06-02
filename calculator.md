<Window x:Class="Calculator.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:Calculator"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="100"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <Grid Grid.Row="0" Margin="5">
            <TextBlock 
                x:Name="textbox" Height="80" Width="100"  HorizontalAlignment="Right"
                 FontSize="50" FontWeight="Bold"/>
        </Grid>
        <Grid Grid.Row="1">
            <Grid.RowDefinitions>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <Button Grid.Row="0" Grid.Column="0" Margin="5" Content="/" Click="button_Click"/>
            <Button Grid.Row="0" Grid.Column="1" Margin="5" Content="*" Click="button_Click"/>
            <Button Grid.Row="0" Grid.Column="2" Margin="5" Content="-" Click="button_Click"/>
            <Button Grid.Row="0" Grid.Column="3" Margin="5" Content="+" Click="button_Click"/>
                                                                       
            <Button Grid.Row="1" Grid.Column="0" Margin="5" Content="7" Click="button_Click" />
            <Button Grid.Row="1" Grid.Column="1" Margin="5" Content="8" Click="button_Click" />
            <Button Grid.Row="1" Grid.Column="2" Margin="5" Content="9" Click="button_Click" />
            <Button Grid.Row="1" Grid.Column="3" Margin="5" Content=""  />

            <Button Grid.Row="2" Grid.Column="0" Margin="5" Content="4" Click="button_Click"/>
            <Button Grid.Row="2" Grid.Column="1" Margin="5" Content="5" Click="button_Click"/>
            <Button Grid.Row="2" Grid.Column="2" Margin="5" Content="6" Click="button_Click"/>
            <Button Grid.Row="2" Grid.Column="3" Margin="5" Content="C" />

            <Button Grid.Row="3" Grid.Column="0" Margin="5" Content="1" Click="button_Click"/>
            <Button Grid.Row="3" Grid.Column="1" Margin="5" Content="2" Click="button_Click"/>
            <Button Grid.Row="3" Grid.Column="2" Margin="5" Content="3" Click="button_Click"/>
            <Button Grid.Row="3" Grid.Column="3" Margin="5" Content="Backspace" />

            <Button Grid.Row="4" Grid.Column="0" Margin="5" Content="+/-"/>
            <Button Grid.Row="4" Grid.Column="1" Margin="5" Content="0"/>
            <Button Grid.Row="4" Grid.Column="2" Margin="5" Content="."/>
            <Button Grid.Row="4" Grid.Column="3" Margin="5" Content="="/>
        </Grid>
    </Grid>
</Window>
