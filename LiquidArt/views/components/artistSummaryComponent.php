<div class="artistSummary">
    <div class="artistSummaryImage">
        <div class="artistSummaryThumbnail">
            <img src="<?php echo $model->Thumbnail; ?>">
        </div>
        <div class="artistSummarySite">
            <a href="<?php echo $model->Site; ?>">View Site</a>
        </div>
    </div>
    <div class="artistSummaryBiography">
        <?php echo $model->Biography; ?>
    </div>
</div>