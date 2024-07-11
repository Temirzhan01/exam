    <script>
    $(document).ready(function() {
        $('#FISum').on('input', function() {
            var value = $(this).val();
            var formattedValue = formatNumber(value);
            $(this).val(formattedValue);
        });

        function formatNumber(value) {
            value = value.replace(/\D/g, '');
            return value.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        }
    });
    </script>
