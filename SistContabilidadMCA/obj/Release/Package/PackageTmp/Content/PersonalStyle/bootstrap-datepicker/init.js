﻿$(function () {


    $.fn.datepicker.dates['es'] = {
        days: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
        daysShort: ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"],
        daysMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
        months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        monthsShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
        today: "Hoy",
        monthsTitle: "Meses",
        clear: "Borrar",
        weekStart: 1,
        format: "dd/mm/yyyy"
    };


    $(".datepicker").datepicker({ autoclose: true, language: 'es' });
    $(".monthpicker").datepicker({ autoclose: true, language: 'es', format: "MM", viewMode: "months", minViewMode: "months" });
    $(".yearpicker").datepicker({ autoclose: true, language: 'es', format: "yyyy", viewMode: "years", minViewMode: "years" });
    $(".monthpicker2").datepicker({ autoclose: true, language: 'es', format: "mm/yyyy", viewMode: "months", minViewMode: "months" });
    $(".monthpicker3").datepicker({ autoclose: true, language: 'es', format: "MM yyyy", viewMode: "months", minViewMode: "months" });


    $('[data-provide="datepicker"]').datepicker({ autoclose: true, language: 'es' });

    $(".datepicker, .monthpicker, .yearpicker, .monthpicker2, .monthpicker3").prop('readonly', true);

    $(".input-group").on('click', function () {
        var datepicker = $(this).find('.datepicker')[0];
        $(datepicker).datepicker('show');
    });

    $(".input-group").on('click', function () {
        var datepicker = $(this).find('.monthpicker')[0];
        $(datepicker).datepicker('show');
    });


    $(".input-group").on('click', function () {
        var datepicker = $(this).find('.monthpicker2')[0];
        $(datepicker).datepicker('show');
    });


    $(".input-group").on('click', function () {
        var datepicker = $(this).find('.monthpicker3')[0];
        $(datepicker).datepicker('show');
    });

    $(".input-group").hover(function () {
        $(this).css("cursor", "pointer");
    });
});
